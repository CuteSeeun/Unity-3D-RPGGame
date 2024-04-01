using Firebase.Auth;
using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NetData : SingletonLazy<NetData>
{
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
        if( _gameData == null)
        {
            TextAsset playerData = Resources.Load<TextAsset>("test");
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
    public IEnumerator SavePlayerDB(FirebaseUser user)
    {
        string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(_gameData.players);

        yield return null;
    }

}
