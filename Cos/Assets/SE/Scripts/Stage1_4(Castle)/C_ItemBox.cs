using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ItemBox : MonoBehaviour
    //온트리거스테이 태그_플레이 -> f키 누르면 박스 열림 
{
    public GameObject ItemBox;
    public GameObject Door;

    private Animator animator;
    public bool isBox;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F) && !isBox)
            {
                isBox = true;
                animator.SetBool("isBox", true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(DisableItemBox());
                    
                }

            }
        }
    }
    IEnumerator DisableItemBox()
    {
        yield return new WaitForSeconds(2f);
        ItemBox.SetActive(false);

        if (Door != null)
        {
            Door.GetComponent<Animator>().Play("Door");
        }
    }
}
