using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;
using Firebase.Database;
using Firebase.Extensions;

namespace KJ
{
    public class FirebaseAuthManager : MonoBehaviour
    {
        [Header("Firebase")]
        // Firebase 를 안전하게 초기화 하고 상태 확인을 위해.
        public DependencyStatus dependencyStatus;
        // 로그인, 회원가입에 사용.
        private FirebaseAuth _auth;
        // 인증이 완료된 유저 정보.
        public FirebaseUser _user { get; private set; }

        [Header("LogIn")]
        // email 입력을 받음.
        public TMP_InputField email;
        // password 입력을 받음.
        public TMP_InputField password;
        // 오류메시지
        public TMP_Text warningLoginText;
        // 성공시 나타나는 메시지
        public TMP_Text confirmLoginText;

        [Header("Register")]
        // username 생성 입력 받음.
        public TMP_InputField usernameRegister;
        // email 생성 입력 받음.
        public TMP_InputField emailRegister;
        // password 생성 입력을 받음.
        public TMP_InputField passwordRegister;
        // password check 입력을 받음.
        public TMP_InputField passwordCheck;
        // 오류 메시지
        public TMP_Text warningRegisterText;
        // 성공시 나타나는 메시지
        public TMP_Text ConfrimRegisterText;

        // 싱글톤 패턴
        private static FirebaseAuthManager _instance;
        public static FirebaseAuthManager Instance
        {
            get
            {
                if (_instance == null)
                {

                }
                return _instance;
            }
        }

        private DatabaseReference _databaseReference;
        void Awake()
        {
            // 싱글톤 패턴
            if (_instance == null)
            {
                _instance = this;
                // 씬 전환시에도 파괴되지 않게 함
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                // 중복 인스턴스 파괴
                Destroy(gameObject);
            }
            // Firebase DependencyStatus 확인함
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                // dependencyStatus 에 변수 저장
                dependencyStatus = task.Result;
                // dependencyStatus 사용 가능한지 확인.
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Firebase 초기화
                    InitializeFirebase();
                    _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                    
                   Debug.Log("firebase initalized successfull");
                }
                else
                {
                    // 오류 메시지 출력.
                    Debug.LogError("Firebase 초기화 실패 : " + dependencyStatus);
                }
            });
        }

        public void WriteData_Data(string userkey)
        {
            DatabaseReference db = null;
            db = FirebaseDatabase.DefaultInstance.GetReference("userinfo");

            /*데이터를 json 으로 변환한다.*/

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("userkey", "bbbbbbbb");
            dic.Add("authdata", "aaaaa");

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(userkey, dic);

            db.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("dataupdate complete");
            });

        }

        public void InitializeFirebase()
        {
            // 초기화
            _auth = FirebaseAuth.DefaultInstance;
        }


        public void LoginButton(Action<bool> onLoginCompleted)
        {
            // 이메일과 비밀번호를 전달하는 로그인 호출.
            StartCoroutine(Login(email.text, password.text, onLoginCompleted));
        }

        public void RegisterButton()
        {
            // 계정 생성할 때 이메일과 비밀번호를 전달하는 Register 호출.
            StartCoroutine(Register(emailRegister.text, passwordRegister.text, usernameRegister.text));
        }

        IEnumerator Login(string _email, string _password, Action<bool> onLoginCompleted)
        {
            // email 과 password 를 전달하여 firebase 인증 함수를 호출.
            var LoginTask = _auth.SignInWithEmailAndPasswordAsync(_email, _password);
            // LoginTask.IsCompleted 가 참이 될 때 까지 기다림.
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
            _user = LoginTask.Result.User;
            
            if (LoginTask.Exception != null)
            {
                // 만약 예외가 발생하여 오류가 나타나면
                Debug.LogWarning(message: $"로그인 하는데 예외가 발생하여 실패 {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "로그인 실패";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "이메일을 적으세요.";
                        break;

                    case AuthError.MissingPassword:
                        message = "비밀번호를 적으세요.";
                        break;

                    case AuthError.WrongPassword:
                        message = "잘못된 비밀번호.";
                        break;

                    case AuthError.InvalidEmail:
                        message = "잘못된 이메일";
                        break;

                    case AuthError.UserNotFound:
                        message = "계정이 존재하지 않습니다.";
                        break;
                }
                warningLoginText.text = message;
                onLoginCompleted?.Invoke(false);

            }
            // 제대로 작동 한다면.
            else
            {
                Debug.LogFormat("로그인 성공 : {0} {1}", _user.Email, _user.DisplayName);
                warningLoginText.text = "";
                confirmLoginText.text = "로그인 성공!!";
                yield return NetData.Instance.LoadPlayerDB(_user, string.Empty);

                onLoginCompleted?.Invoke(true);
            }
        }

        IEnumerator Register(string _email, string _password, string _username)
        {
            if (_username == "")
            {
                warningRegisterText.text = "닉네임을 정해주세요.";
            }
            else if (passwordRegister.text != passwordCheck.text)
            {
                warningRegisterText.text = " 비밀번호가 일치하지 않습니다. 다시 시도하세요.";
            }
            else
            {
                var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                // 계정을 만들 때 오류가 발생하면
                if (RegisterTask.Exception != null)
                {
                    Debug.LogWarning(message: $"등록하는데 예외가 발생하여 실패 {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "회원가입 실패!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "이메일을 적으세요.";
                            break;

                        case AuthError.MissingPassword:
                            message = "비밀번호를 적으세요.";
                            break;

                        case AuthError.WeakPassword:
                            message = "비밀번호의 보안이 취약합니다.";
                            break;

                        case AuthError.EmailAlreadyInUse:
                            message = "중복된 이메일입니다.";
                            break;
                    }
                    warningRegisterText.text = message;
                }
                // 회원가입 성공.
                else
                {
                    _user = RegisterTask.Result.User;

                    if (_user != null)
                    {
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        var ProfileTask = _user.UpdateUserProfileAsync(profile);

                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            // 사용자 정보 (username) 을 불러오는데 예외가 발생하면
                            Debug.LogWarning(message: $"사용자 프로필 정보를 업데이트 하는데 예외가 발생했습니다. {ProfileTask.Exception}");
                            FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                            warningRegisterText.text = "유저 네임을 불러오는데 실패했습니다!";
                        }
                        // 성공한다면
                        else
                        {
                            Debug.Log("회원가입이 성공적으로 이루어졌습니다." + _user.DisplayName);
                            ConfrimRegisterText.text = "회원가입 성공!!";
                            warningRegisterText.text = "";
                        }
                    }
                }
            }
        }
    }

}
