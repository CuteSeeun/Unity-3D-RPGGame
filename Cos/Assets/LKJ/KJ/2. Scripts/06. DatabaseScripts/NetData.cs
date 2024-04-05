using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using KJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class NetData : SingletonLazy<NetData>
{
    public GameData gameData { get; private set; }
    public GameData _gameData { get; private set; }


    private IEnumerator Start()
    {
        //TextAsset playerData = Resources.Load<TextAsset>("test");
        //_gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(playerData.text);

        yield return null;
    }

    public IEnumerator LoadPlayerDB()
    {

        //TextAsset playerData = Resources.Load<TextAsset>("test");
        //_gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(playerData.text);

        yield return null;
    }


    public IEnumerator LoadPlayerDB(FirebaseUser user, string jsondata)
    {
        if (gameData == null && _gameData == null)
        {
            TextAsset classData = Resources.Load<TextAsset>("ClassDB");
            gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(classData.text);

            TextAsset playerData = Resources.Load<TextAsset>("PlayerDB");
            _gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(playerData.text);
        }

        //_gameData.players = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Player>>(jsondata);
        Debug.Log("LoadPlayerDB(FirebaseUser user, string jsondata)");
        Player p = new Player();
        p.uid = user.UserId;
        p.shortUID = UIDHelper.GenerateShortUID(user.UserId);

        //PlayerDBManager.Instance.SaveOrUpdatePlayerData(p.uid, p);

        yield return null;
    }

    public void ReadDataPlayer(string userkey, Action callback = null)
    {
        DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("players");

        db.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError("ReadData  IsFaulted");
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("childerenCount" + snapshot.ChildrenCount);

                //DataSnapshot duserinfo = null;
                //foreach (var child in snapshot.Children)
                //{
                //    if (child.Child("player").Value.ToString().Equals(userkey))
                //    {
                //        duserinfo = child;
                //    }
                //}

                //if (duserinfo != null)
                //{
                /*유저 데이터를 복원 한다.*/
                string strplayerdata = snapshot.Value.ToString();
                if (!string.IsNullOrEmpty(strplayerdata))
                {
                    Debug.Log("복원완료!" + _gameData);
                    _gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(strplayerdata);
                }
                //}

                if (callback != null)
                {
                    callback();
                }
            }
        });

    }

    public void ReadDataItem(string userkey, Action callback = null)
    {
        DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("Items");

        db.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError("ReadData  IsFaulted");
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("childerenCount" + snapshot.ChildrenCount);

                //DataSnapshot duserinfo = null;
                //foreach (var child in snapshot.Children)
                //{
                //    if (child.Child("Items").Value.ToString().Equals(userkey))
                //    {
                //        duserinfo = child;
                //    }
                //}

                //if (duserinfo != null)
                //{
                /* 아이템 데이터를 복원. */
                string strItemdata = snapshot.Value.ToString();
                Debug.Log($"{!string.IsNullOrEmpty(strItemdata)}");
                if (!string.IsNullOrEmpty(strItemdata))
                {
                    Debug.Log("복원완료2!" + ItemDBManager.Instance._itemData);
                    ItemDBManager.Instance._itemData = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemData>(strItemdata);
                }
                //}

                if (callback != null)
                {
                    callback();
                }
            }
        });

    }

    public void SavePlayerDB(FirebaseUser user)
    {
        string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(_gameData.players);

        WritePlayerData(_gameData.players.Values.ToList()[0].uid, "players", jsondata);

        string jsondata2 = Newtonsoft.Json.JsonConvert.SerializeObject(ItemDBManager.Instance._itemData.items);

        Debug.Log($"{ItemDBManager.Instance._itemData.items[0].id}");
        WriteItemData(ItemDBManager.Instance._itemData.items[0].id, "Items", jsondata2);



    }
    public void WritePlayerData(string usermail, string datakey, string jsondata)
    {
        //string json = Newtonsoft.Json.JsonConvert.SerializeObject(ItemDBManager.Instance._itemData);

        DatabaseReference db = null;
        db = FirebaseDatabase.DefaultInstance.GetReference(usermail);

        /*데이터를 json 으로 변환한다.*/

        //Dictionary<string, object> dic = new Dictionary<string, object>();
        //dic.Add(datakey, jsondata);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add(datakey, jsondata);

        db.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("dataupdate complete");
        });

    }

    public void WriteItemData(string itemdb, string idatakey, string jsondata2)
    {
        //string json = Newtonsoft.Json.JsonConvert.SerializeObject(ItemDBManager.Instance._itemData);

        DatabaseReference db = null;
        db = FirebaseDatabase.DefaultInstance.GetReference(itemdb);

        /*데이터를 json 으로 변환한다.*/

        //Dictionary<string, object> dic = new Dictionary<string, object>();
        //dic.Add(datakey, jsondata);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add(idatakey, jsondata2);

        db.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("dataupdate complete");
        });

    }
}
