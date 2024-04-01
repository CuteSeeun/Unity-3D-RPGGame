using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator animator;

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
}
