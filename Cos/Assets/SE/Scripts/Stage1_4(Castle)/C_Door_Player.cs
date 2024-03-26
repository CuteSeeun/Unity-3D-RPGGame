using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Door_Player : MonoBehaviour
{//온트리거 스테이 -> 태그 플레이 -> 이프 애너미 태그 없으면 -> f키 눌렀을 때 문이 열림

    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘

    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpen = true;
                door.SetBool("isOpen", true);
            }
        }
    }
}
