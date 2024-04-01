using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissleHitImpact : MonoBehaviour
{
    public GameObject Hitimpact;
    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            Instantiate(Hitimpact, transform.position, Quaternion.identity);
        }

    }
}
