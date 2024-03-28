using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Collider col;
    void Start()
    {
        Invoke("Remove", 3);
        Invoke("Col", 0.75f);
        col = GetComponent<CapsuleCollider>();
        col.enabled = false;
    }

    void Update()
    {
        
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    void Col()
    {
        col.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 데미지 주기
        }
    }
}
