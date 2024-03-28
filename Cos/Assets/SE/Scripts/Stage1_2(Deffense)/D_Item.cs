using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class D_Item : MonoBehaviour
{
    public GameObject DoorLock;

    public GameObject ItemLock;
    public GameObject gate;
    public GameObject gate2;
    public GameObject gate3;
    public GameObject gate4;
    public GameObject gate5;
    public GameObject gate6;

    private Animator gateAnimator;
    private Animator gateAnimator2;
    private Animator gateAnimator3;
    private Animator gateAnimator4;
    private Animator gateAnimator5;
    private Animator gateAnimator6;

    void Start()
    {
        gateAnimator = gate.GetComponent<Animator>();
        gateAnimator2 = gate2.GetComponent<Animator>();
        gateAnimator3 = gate3.GetComponent<Animator>();
        gateAnimator4 = gate4.GetComponent<Animator>();
        gateAnimator5 = gate5.GetComponent<Animator>();
        gateAnimator6 = gate6.GetComponent<Animator>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemLock.SetActive(false);

                if (gateAnimator != null)
                {
                    gateAnimator.SetTrigger("isOpen");
                    gateAnimator2.SetTrigger("isOpen");
                    gateAnimator3.SetTrigger("isOpen");
                    gateAnimator4.SetTrigger("isOpen");
                    gateAnimator5.SetTrigger("isOpen");
                    gateAnimator6.SetTrigger("isOpen");

                    Invoke("DestroyGameObject", 15f);
                }
            }
        }

    }

    private void DestroyGameObject()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
            DoorLock.SetActive(false);
        }
    }
}
