using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Animator anime;

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag.Equals("Player"))
        {
            
                anime.SetTrigger("shaker");
        }
    }
    private void Start()
    {
        anime = GetComponent<Animator>();
    }
}
