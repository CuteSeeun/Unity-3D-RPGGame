using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public class ItemData
//{

//}
//public class PlayersData 
//{
//    public string _uid;
//    public string _userName;
//    public string _class;
//    public List<ItemData> _inventory;
//    public uint _gold;
//    public uint _combatPower;
//    public List<int> _buffs;
//}

//public class Class
//{
//    public string _name;
//    public int _baseHp;
//    public int _baseSp;
//    public uint _gold;
//    public class Item
//    {
//        public string _idx;
//        public int _count;
//    }

//    public List<Item> _inventory;

//    public Class(string name, int baseHp, int baseSp, uint gold, List<Item> inventory)
//    {
               
//        _name = name;   
//        _baseHp = baseHp;
//        _baseSp = baseSp;
//        _gold = gold;
//        _inventory = inventory;
//    }
//}

//public class ClassesData
//{
//    public List<Class> _classes;


//    public ClassesData()
//    {
//        _classes = new List<Class>();


//        List<Class.Item> items = new List<Class.Item>();
//        Class.Item i = null;
//        i = new Class.Item();
//        i._idx = "11112";
//        i._count = 1;
//        items.Add(i);

//        i = new Class.Item();
//        i._idx = "11124";
//        i._count = 0;
//        items.Add(i);

//        Class knight = new Class("knight", 100, 100, 100, items);
       
//    }

//}


/*
 "players": {
    "UID": {
      "userName": "",
      "class": "",
      "inventory": {
        "items": []
      },
      "gold": 0,
      "combatPower": 0,
      "buffs": []
    }
  },
  "classes": 
{
    "knight": {
      "name": "Knight",
      "baseHp": 100,
      "baseSP": 100,
      "inventory": {
        "items": [ "11112", "11124" ],
        "11112": 1,
        "11124": 1
      },
      "gold": 100
    },
    "barbarian": {
      "name": "Barbarian",
      "baseHP": 100,
      "baseSP": 100,
      "inventory": {
        "items": [ "11115", "11124" ],
        "11115": 1,
        "11124": 1
      },
      "gold": 100
    },
    "rogue": {
      "name": "Rogue",
      "baseHP": 100,
      "baseSP": 100,
      "inventory": {
        "items": [ "11118", "11124" ],
        "11118": 1,
        "11124": 1
      },
      "gold": 100
    },
    "mage": {
      "name": "Mage",
      "baseHP": 100,
      "baseSP": 100,
      "inventory": {
        "items": [ "11121", "11124" ],
        "11121": 1,
        "11124": 1
      },
      "gold": 100
    }
  }*/