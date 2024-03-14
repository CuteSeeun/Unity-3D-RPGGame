using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Player.Scripts.FSM
{
    public enum State
    {
        Idle,
        Move,
        Dodge,
        Attack1,
        Attack2,
        HitA,
        HitB,
        Death,
        Interact,
        PickUp,
        UseItem
    }
}
