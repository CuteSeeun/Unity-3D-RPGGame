using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘

    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }
    
    private void OnTriggerStay(Collider other)
    {
            if(Input.GetKeyDown(KeyCode.F)) 
            {
                isOpen = true;
                door.SetBool("isOpen", true);
            }
     }
}
