using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DoorGate : MonoBehaviour
{
    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘
    

    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }
    public void ToggleDoor() //문 열고 닫는 함수
    {
        isOpen = !isOpen; //열려라 참깨. 닫혀있다면 열려라.
        door.SetBool("isOpen", true); //파라미터 설정.
        
    }

    void CheckDoorConditions() //열림 닫힘 조건을 체크하는 함수
    {
        //F키를 누르면 ToggleDoor() 함수를 호출하도록 함
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleDoor();
        }
    }

    private void Update()
    {
        CheckDoorConditions();

        if (!isOpen)
        {
            door.SetBool("isOpen", false);
        }
        else if (isOpen)
        {
            door.SetBool("isOpen", true);
        }
    }
}
