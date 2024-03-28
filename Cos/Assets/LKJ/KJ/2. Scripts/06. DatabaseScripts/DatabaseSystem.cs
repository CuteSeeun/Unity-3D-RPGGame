using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace KJ
{
    [System.Serializable]
    public class ItemData
    {
        public string type;
        public string id;
        public string name;
        public string description;
        public int quantity;
        public string imagePath;
        public bool enhanceable;
        public string enhanceLevel;
        public Attribute attribute;
    }

    [System.Serializable]
    public class Attribute
    {
        public float healthRestore;
        public float attackBuff;
        public float defenseBuff;
        public float staminaRecovery;
        public List<string> applicableItemType = new List<string>();
        public int successRateIncrease;
        public int maxUseLevel;
        public List<string> usedIn = new List<string>();
    }

    public class PlayerData
    {

    }

    public class Class
    {

    }

    public class ClassData
    {

    }










    /* UID 를 해시 함수를 통해 간결한 ShortUID 로 변환, */
    public static class UIDHelper
    {
        public static string GenerateShortUID(string longUID)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                /* SHA256 해시값을 계산. */
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(longUID));

                /* 바이트 배열을 string 으로 변환. */
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                /* 해시값의 일부분만 사용하여 ShortUID 생성. */
                return builder.ToString().Substring(0, 8);  // 8자리만 사용.
            }
        }
    }
    public class DatabaseSystem : MonoBehaviour
    {
        void Start()
        {
            /* Json 파일로부터 데이터 로드 */
            TextAsset jsonData = Resources.Load<TextAsset>("PlayerDB");
            //GameData gameData = JsonUtility.FromJson<GameData>(jsonData.text);
        }
        void Update()
        {

        }
    }

}
