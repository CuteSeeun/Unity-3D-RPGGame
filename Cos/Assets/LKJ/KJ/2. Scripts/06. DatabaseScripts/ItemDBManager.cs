using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /* MonoBehaviour 를 상속받고, 제네릭을 사용하여 싱글톤 인스턴스 생성.
     반드시 T 가 class 이여야 함. */
    public class SingletonLazy<T> : MonoBehaviour where T : class
    {
        /* Lazy<T> 를 사용하여 싱글톤 인스턴스를 저장.
         Lazy 는 실제로 값이 필요할 때까지 생성을 지연시키는 객체 */
        private static readonly Lazy<T> _instance = 
            new Lazy<T>(() =>
        {
            /* 현재 씬에서 T 타입의 객체를 찾음. */
            T instance = FindObjectOfType(typeof(T)) as T;

            /* 인스턴스가 null 이면 새로운 GameObject 를 생성하고 T 컴포넌트 추가 */
            if (instance == null)
            {
                GameObject obj = new GameObject("SingletonLazy");
                instance = obj.AddComponent(typeof(T)) as T;

                /* 씬 전환시 Gameobject가 파괴되지 않도록 설정. */
                DontDestroyOnLoad(obj);
            }
            else
            {
                /* 이미 인스턴스가 있을 경우 새로 생성된 GameObject 파괴 */
                Destroy(instance as GameObject);
            }

            /* 생성된 인스턴스 반환. */
            return instance;
        });

        /* 싱글톤 인스턴스에 접근하기 위한 정적 속성. */
        public static T Instance
        {
            get 
            { 
                /* Lazy 인스턴스의 Value 속성을 통해 싱글톤 인스턴스 반환
                 접근 하는 순간 Lazy 객체가 실제 값을 생성. */
                return _instance.Value; 
            }
        }
    }


    /* SingletonLazy<T> 를 상속받아서 싱글톤 구현하는 클래스 */
    public class ItemDBManager : SingletonLazy<ItemDBManager>
    {
        public ItemData _itemData { get; set; }

        public Item GetItem(string id) 
        {
            return _itemData.items.Find(x => x.id == id);
        }

        public IEnumerator LoadItemDB()
        {
            Debug.Log(" 로드 완료 " + _itemData);
            TextAsset itemData = Resources.Load<TextAsset>("ItemDB");
            Debug.Log(" 로드 완료 " + itemData.text);
            _itemData = JsonUtility.FromJson<ItemData>(itemData.text);

            //yield return new WaitForSeconds(0.5f);
            //_itemData = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemData>(itemData.text);

            yield return null;
        }

        Dictionary<string, Sprite> _loadpool = new Dictionary<string, Sprite>();
        public Sprite LoadItemSprite(string imagePath)
        {
            Sprite s = Resources.Load<Sprite>(imagePath);
            if (s == null) Debug.Log("LoadItemSprite == null : " + imagePath);
            return s;
        }


        //private DatabaseReference _databaseReference;
        //private void Start()
        //{
        //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //    {
        //        if (task.IsFaulted)
        //        {
        //            Debug.LogError($"failed to initialize firebase with {task.Exception}");
        //            return;
        //        }

        //        var dependencyStatus = task.Result;
        //        if (dependencyStatus == Firebase.DependencyStatus.Available)
        //        {
        //            FirebaseApp app = FirebaseApp.DefaultInstance;

        //            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //            //_auth = FirebaseAuth.DefaultInstance;
        //            //_auth.StateChanged += OnAuthStateChanged;

        //            /*초기화 결과 확인*/
        //            Debug.Log("firebase initalized successfull");

        //        }
        //        else
        //        {
        //            // 실패
        //            Debug.LogError($"Firebase dependencies : {dependencyStatus}");
        //        }

        //    });
        //}
    }
}