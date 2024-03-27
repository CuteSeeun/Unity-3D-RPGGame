using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    

    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘


    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }
    public void OpenDoor() //문 열고 닫는 함수
    {
        isOpen = true; //열려라 참깨. 닫혀있다면 열려라.
        door.SetBool("isOpen", true); //파라미터 설정.

    }

    void CheckDoorConditions() //열림 조건을 체크하는 함수
    {
        //F키를 누르면 ToggleDoor() 함수를 호출하도록 함
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();
        }
    }

    private void Update()
    {
        CheckDoorConditions();


        if (isOpen)
        {
            door.SetBool("isOpen", true);
        }
    }


}
