using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Door_Enemy : MonoBehaviour
{//온트리거스테이 아이템 태그 없으면 문이 열림
    private Animator door;
    public bool isOpen;

    private void Start()
    {
        door = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Item"))
        {
            isOpen = true;
            door.SetBool("isOpen", true);
        }
    }
}
