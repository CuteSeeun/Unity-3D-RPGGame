using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject item; // 아이템 프리팹
    public Transform itemSpawn;//아이템 생성 위치
    public float openSpeed = 1.0f;
    public float delayBeforeDisappear = 1.0f;

    private Animator boxAnimator;
    private bool Break = false;

    void Start()
    {
        boxAnimator = GetComponent<Animator>();
        if (boxAnimator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Break)
        {
            StartCoroutine(OpenBox());
        }
    }

    IEnumerator OpenBox()
    {
        // 박스 열기 애니메이션 재생
        boxAnimator.SetBool("Break", true);
        Break = true;

        // 열기 애니메이션 시간만큼 대기
        yield return new WaitForSeconds(openSpeed);

        // 아이템 활성화
        item.SetActive(true);

        // delayBeforeDisappear 시간만큼 대기 후 박스 사라지는 애니메이션 시작
        yield return new WaitForSeconds(delayBeforeDisappear);

        // 박스 사라지는 애니메이션 재생
        boxAnimator.SetBool("IsOpen", false);

        // 박스 사라진 후 아이템 비활성화
        item.SetActive(false);

        Break = false;
    }




}
