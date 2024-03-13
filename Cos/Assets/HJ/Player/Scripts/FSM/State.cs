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
        Hit,
        Death,
        Interact,
        PickUp,
        UseItem
    }
}
