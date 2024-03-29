using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Collider col;
    void Start()
    {
        Invoke("Remove", 1.5f);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(5, true, Quaternion.identity);
        }
    }
}
