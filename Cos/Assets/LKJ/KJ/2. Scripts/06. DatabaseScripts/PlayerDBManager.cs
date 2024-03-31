using KJ;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
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



    public class PlayerDBManager : SingletonLazy<PlayerDBManager>
    {
        GameData gameData = GameData.Instance;

        public IEnumerator LoadPlayerDB()
        {
            Debug.Log(" 로드 완료 " + gameData);
            TextAsset playerData = Resources.Load<TextAsset>("PlayerDB");
            gameData = JsonUtility.FromJson<GameData>(playerData.text);

            yield return null;
        }

        public string CurrentShortUID { get; private set; }

        public void SaveOrUpdatePlayerData(string UID, Player playerData)
        {
            string shortUID = UIDHelper.GenerateShortUID(UID);
            CurrentShortUID = shortUID;

            playerData.shortUID = shortUID;

            if (gameData.players.ContainsKey(shortUID))
            {
                gameData.players[shortUID] = playerData;
            }
            else
            {
                gameData.players.Add(shortUID, playerData);
            }
        }

        public bool CheckPlayerData(string shortUID)
        {
            if (string.IsNullOrEmpty(shortUID)) return false;
            return gameData.players.ContainsKey(shortUID);
        }

        public Player LoadGameData(string shortUID)
        {
            if (gameData.players.TryGetValue(shortUID, out Player playerData))
            {
                /* 플레이어 위치나 인벤토리 그리고 능력치 및 설정 저장. */
                return playerData;
            }
            else
            {
                /* 새 플레이어 생성 로직 추가. */
                return null;
            }
        }
    }
}


