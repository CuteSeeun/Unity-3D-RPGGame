using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KJ
{
    /*************************************************************************************/
    /// <summary>
    /// PlayerDB 와 ItemDB Json 파일을 정리한 것.
    /// PlayerDB 에는 ClassDB 가 ClassDB 안에는 SkillDB가 들어가 있음.
    /// </summary>
    /************************************************************************************/
    
    /* ItemDB.Json 종합적으로 관리 */
    [System.Serializable]
    public class ItemData
    {
        public List<Item> items = new List<Item> ();
    }   

    /* 아이템 데이터 */
    [System.Serializable]
    public class Item
    {
        public string type;
        public string id;
        public string name;
        public string description;
        public int quantity;
        public string imagePath;
        public bool enhanceable;
        public int enhanceLevel;
        public Attribute attributes;
    }

    /* 아이템 데이터의 Atrribute 데이터 */
    [System.Serializable]
    public class Attribute
    {
        public float healthRestore = 0;
        public float attackPowerUp = 0;
        public float defensePowerUp = 0;
        public float staminaRecovery = 0;
        public float successRateIncrease = 0;
        public int maxUseLevel = 0;
        public List<string> usedIn = new List<string>();
        public List<ItemRequirement> quantityRequired = new List<ItemRequirement>();
        public float attack = 0;
        public float defense = 0;
        public float skillDamage = 0;
    }

    [System.Serializable]
    public class ItemRequirement
    {
        public string id;
        public int quantity;
    }

    /* PlayerDB.Json 종합적으로 관리 */
    [System.Serializable]
    public class GameData 
    {
        /* 플레이어 데이터를 저장할 Dictionary. */
        public Dictionary<string, Player> players = new Dictionary<string, Player>();
        /* 클래스 데이터를 저장할 Dictionary. */
        public Dictionary<ClassType, Class> classes = new Dictionary<ClassType, Class>();

    }

    [System.Serializable]
    /* Player 데이터 */
    public class Player
    {
        public string uid;
        public string shortUID;
        public string userName;
        public string classType;
        public Inventory inventory;
        public int gold;
        public float combatPower;
        public List<string> buffs;

        public Player()
        {
            inventory = new Inventory();
            buffs = new List<string>();
        }
    }

    /* 인벤토리 관리 */
    [System.Serializable]
    public class Inventory
    {
        public List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }
    }

    /* 인벤토리 데이터 */
    [System.Serializable]
    public class InventoryData
    {
        public string id;
        public int quantity;
    }

    /* 클래스 enum 타입으로 분류 */
    public enum ClassType
    {
        knight,
        barbarian,
        rogue,
        mage
    }

    /* Class 데이터 */
    [System.Serializable]
    public class Class
    {
        public ClassType classType;
        public string name;
        public float baseHp;
        public float baseSp;
        public Inventory inventory;
        public List<Skill> skills;
        public int gold;

        public Class()
        {
            inventory = new Inventory();
            skills = new List<Skill>();
        }

    }

    /* Skill 데이터 */
    [System.Serializable]
    public class Skill
    {
        public string id;
        public string name;
        public string description;
    }
}
