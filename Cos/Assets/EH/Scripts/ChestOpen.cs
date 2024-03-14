using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public Animator animator;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator.GetBool("isOpenChest") == false)
            {
                animator.SetBool("isOpenChest", true);
            }
        }

    }
}
