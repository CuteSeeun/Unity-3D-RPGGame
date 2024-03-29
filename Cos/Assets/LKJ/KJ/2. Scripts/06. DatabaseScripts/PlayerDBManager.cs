using KJ;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace KJ
{ 
    /* UID 를 해시함수를 이용해서 ShortUID 로 변환. */
    public static class UIDHelper
    {
        public static string GenerateShortUID(string longUID)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                /* SHA256 해시 값을 계산. */
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(longUID));

                /* 바이트 배열을 String 으로 변환. */
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                /* 해시값의 앞부분만 사용하여 ShortUID 생성. */
                return builder.ToString().Substring(0, 8);  // 앞의 8자리만 사용
            }
        }
    }

    public class PlayerDBManager : MonoBehaviour
    {
        public static PlayerDBManager Instance { get; private set; }


        void Awake()
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

        void DataLoad()
        {
            /* 데이터 로드, Player 로드하면 class 도 같이 로드됨 */
            TextAsset playerData = Resources.Load<TextAsset>("PlayerDB");
            /* Passing */
            GameData gameData = JsonUtility.FromJson<GameData>(playerData.text);

        }

    }
}


