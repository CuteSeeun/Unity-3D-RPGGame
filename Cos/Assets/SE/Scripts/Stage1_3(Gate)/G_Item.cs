using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Item : MonoBehaviour
{
    public GameObject G_DoorItemEffect;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                G_DoorItemEffect.SetActive(true); //이펙트가 꺼져있는게 기본값
            }
        }
    }
}
