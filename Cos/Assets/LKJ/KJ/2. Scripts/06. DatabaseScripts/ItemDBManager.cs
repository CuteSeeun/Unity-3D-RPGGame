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

                DontDestroyOnLoad(obj);
            }
            else
            {
                Destroy(instance as GameObject);
            }

            return instance;
        });

        public static T Instance
        {
            get 
            { 
                return _instance.Value; 
            }
        }
    }

    public class ItemDBManager : SingletonLazy<ItemDBManager>
    {
        public Dictionary<string, Item> itmes = new Dictionary<string, Item>();
    }
}