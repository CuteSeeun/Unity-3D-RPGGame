using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ItemDBManager : MonoBehaviour
    {
        public static ItemDBManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                /* Scene 전환시 파괴 방지 */
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                /* 중복 인스턴스 생성 방지 */
                Destroy(gameObject);
            }
        }

        public Dictionary<string, Item> itmes = new Dictionary<string, Item>();
    }
}