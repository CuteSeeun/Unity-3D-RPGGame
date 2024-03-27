using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Key : MonoBehaviour
{
    public GameObject Gate;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Gate 게임 오브젝트의 Animator 컴포넌트 가져오기
                Animator gateAnimator = Gate.GetComponent<Animator>();

                // 만약 Gate 게임 오브젝트에 Animator 컴포넌트가 존재한다면
                if (gateAnimator != null)
                {
                    // "Open"이라는 이름의 애니메이션을 실행
                    gateAnimator.Play("P_Gate");
                }
                Destroy(gameObject);
            }
        }
    }

}
