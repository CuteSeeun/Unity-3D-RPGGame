using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
     
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if( Input.GetKeyDown(KeyCode.F) )
        {
            if (animator.GetBool("isOpen") == false)
            {
                animator.SetBool("isOpen", true);
            }
        }
        
    }
    //private void OnCollisionExit(Collision collision)
    //{

    //}
}
