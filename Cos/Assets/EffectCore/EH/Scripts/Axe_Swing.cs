using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Swing : MonoBehaviour
{
    public int damage = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            return;
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            //플레이어에게 데미지를 주며 넉백시킨다.
        }
    }

}
