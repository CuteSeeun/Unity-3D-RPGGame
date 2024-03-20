using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    #region 설정창 열고 닫기
    [Header("SettingUI On/Off")]
    [SerializeField] private GameObject _settingUI;

    public void OpenSettingUI()
    {
        _settingUI.SetActive(true);
    }

    public void CloseSettingUI()
    {
        _settingUI.SetActive(false);
    }
    #endregion
    #region 게임 시작시 로그인 창 열기, X 누르면 닫기
    [Header("Login On/Off")]
    [SerializeField] private GameObject _logInUI;

    public void OpenLogIn()
    {
        _logInUI.SetActive(true);
    }

    public void CloseLogIn()
    {
        _logInUI.SetActive(false); 
    }
    #endregion
    #region 게임 종료 (현재는 테스트씬으로 이동)
    public void Quit()
    {
        SceneManager.LoadScene("TestGameQuit");
    }
    #endregion
    #region 로그인 하면 로딩창으로 이동
    
    public void AttemptLogin()
    {
        FirebaseAuthManager.Instance.LoginButton(Result =>
        {
            if (Result)
            {
                SceneManager.LoadScene("LoadingScene");
            }
            else
            {
                Debug.LogError("로그인 실패");
            }
        });
              
    }
    #endregion
    #region 회원가입창 열고 닫기
    [Header("RegisterUI On/Off")]
    [SerializeField] private GameObject _registerUI;

    public void OpenRegister()
    {
        _registerUI.SetActive(true);
    }

    public void CloseRegister()
    {
        _registerUI.SetActive(false);
    }
    #endregion
    #region InputField 이미지 숨기기
    // InputField 와 Image 를 1대1 매칭하기 위한 Dictionary.
    private Dictionary<TMP_InputField, GameObject> _inputFieldImageMap = new Dictionary<TMP_InputField, GameObject>();

    [Header("ImageToHide")]
    /* Login */
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    /* Register */
    public TMP_InputField usernameRegister;
    public TMP_InputField emailRegister;
    public TMP_InputField passwordRegister;
    public TMP_InputField passwordCheckRegister;

    [Header("Image")]
    /* Login */
    public GameObject emailLoginImage;
    public GameObject passwordLoginImage;
    /* Register */
    public GameObject usernameRegisterImage;
    public GameObject emailRegisterImage;
    public GameObject passwordRegisterImage;
    public GameObject passwordCheckImage;
    #endregion
    #region Tab 키로 InputField 포커스 이동
    // 현재 활성화 된 패널 식별하는 enum.    
    public enum ActivePanel {Login, Register}
    [Header("InputField Focus")]
    public ActivePanel activePanel;

    
    // 로그인 InputFields
    public List<TMP_InputField> inputFieldsLogin;
    // 회원가입 InputFields
    public List<TMP_InputField> inputFieldsRegister;
    #endregion
    #region 이메일 저장
    [Header("SaveEmail")]
    // 이메일 InputField.
    public TMP_InputField emailInputField;
    // 이메일 저장 토글
    public Toggle saveEmailToggle;

    // PlayerPrefs 에서 이메일 저장할때 사용할 키
    private const string EmailKey = "UserEmail";


    #endregion

    void Start()
    {
        #region InputField 이미지 숨기기
        // 각 InputField 와 Image 매칭
        /* -----------------------------Login------------------------------- */
        _inputFieldImageMap.Add(emailLogin, emailLoginImage);
        _inputFieldImageMap.Add(passwordLogin, passwordLoginImage);
        /* ---------------------------Register------------------------------ */
        _inputFieldImageMap.Add(usernameRegister, usernameRegisterImage);
        _inputFieldImageMap.Add(emailRegister, emailRegisterImage);
        _inputFieldImageMap.Add(passwordRegister, passwordRegisterImage);
        _inputFieldImageMap.Add(passwordCheckRegister, passwordCheckImage);

        // 모든 InputField 에 Listener 추가.
        foreach (var pair in _inputFieldImageMap)
        {
            pair.Key.onValueChanged.AddListener((value) => ToggleImage(pair.Key, value));
        }
        #endregion
        #region 이메일 저장
        // 앱 시작시 저장된 이메일 불러오기.
        LoadEmail();

        // 토글 버튼에 따라 이메일 저장 여부 결정.
        saveEmailToggle.onValueChanged.AddListener(OnToggleChanged);

        #endregion
    }

    void Update()
    {
        #region Tab 키로 InputField 포커스 이동
        // tab 키를 누르면 NavigateThroughInputField 메서드 실행.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 탭별로 활성화
            switch (activePanel)
            {
                case ActivePanel.Login:
                    NavigateThroughInputField(inputFieldsLogin);
                    break;
                case ActivePanel.Register:
                    NavigateThroughInputField(inputFieldsRegister);
                    break;
            }
            
        }
        #endregion
    }

    #region Tab 키로 InputField 포커스 이동

    /* InputField 포커스 설정 */
    public void NavigateThroughInputField(List<TMP_InputField> inputFields)
    {
        for (int i = 0; i < inputFields.Count; i++)
        {
            if (inputFields[i].isFocused)
            {
                // 다음 InputField 계산.
                int nextIndex = (i + 1) % inputFields.Count;
                // 다음 InputField 에 포커스 설정.
                inputFields[nextIndex].Select();
                // 다음 InputField 활성화.
                inputFields[nextIndex].ActivateInputField();
                break;
            }
        }
    }

    // InputField 포커스 활성화 패널
    public void SetActivePanel(ActivePanel panel)
    {
        activePanel = panel;
    }

    // InputField Login 창에서 포커스 활성화.
    public void SetActivePanelToLogin()
    {
        SetActivePanel(LoginManager.ActivePanel.Login);
    }
    // InputField Register 창에서 포커스 활성화.
    public void SetActivePanelToRegister()
    {
        SetActivePanel(LoginManager.ActivePanel.Register);
    }
    #endregion

    #region InputField 이미지 숨기기
    // 이미지 활성화 / 비활성화 하는 메서드
    void ToggleImage(TMP_InputField inputField, string inputValue)
    {
        // 해당 InputField 에 연결된 이미지 활성화 / 비활성화.
        if (_inputFieldImageMap.TryGetValue(inputField, out GameObject Image))
        {
            Image.SetActive(string .IsNullOrEmpty(inputValue));
        }
    }
    #endregion

    #region 이메일 저장
    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // 토글이 On 일때 현재 InputField 의 이메일 주소 저장.
            SaveEmail();
        }
        else
        {
            // 토글이 Off 일때 저장된 이메일 주소 삭제.
            PlayerPrefs.DeleteKey(EmailKey);
        }
    }

    public void SaveEmail()
    {
        PlayerPrefs.SetString(EmailKey, emailInputField.text);
        PlayerPrefs.Save();
    }

    public void LoadEmail()
    {
        if (PlayerPrefs.HasKey(EmailKey))
        {
            string saveEmail = PlayerPrefs.GetString(EmailKey);
            emailInputField.text = saveEmail;
            // 저장된 이메일이 있으면 토글이 true 인 상태로 설정.
            saveEmailToggle.isOn = true;
        }
        else
        {
            // 저장된 이메일이 없으면 토글이 false 인 상태로 설정.
            saveEmailToggle.isOn = false;
        }
        
    }
    #endregion
}
