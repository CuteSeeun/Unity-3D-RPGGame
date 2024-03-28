using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Door : MonoBehaviour
{
    // 문 열리는 조건 : item 모두 null일때

    public GameObject Item1;
    public GameObject Item2;


    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘

    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }

    private void Update()
    {
        // item이 모두 null 일때 애니메이션 재생
        if (Item1 == null && Item2 == null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpen = true;
                door.SetBool("isOpen", true);
            }
        }
    }
}
