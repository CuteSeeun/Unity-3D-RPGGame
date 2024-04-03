using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

public class BossMagicBallHit : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(5, true, transform.rotation);
        }
    }
}
