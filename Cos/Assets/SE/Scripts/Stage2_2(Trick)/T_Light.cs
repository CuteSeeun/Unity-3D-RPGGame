using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Light : MonoBehaviour
{
    //자기 조명 킬거임. 그 조명 초기에 !enabled. f키 누르면 enabled. enabled되면 문 애니메이션 재생.
    Light myLight;
    public GameObject Door;
    private void Start()
    {
        myLight = GetComponent<Light>();
        myLight.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                myLight.enabled = !myLight.enabled;

                Animator animator = Door.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("isOpen");
                }
            }
        }
    }
}
