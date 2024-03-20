using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{

    public GameObject Smog;
    public GameObject Shake;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Smogtrigger()
    {
        Instantiate(Smog, transform.position, Quaternion.identity);
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Destroy(Shake, 2f);
            Invoke("Smogtrigger", 2f);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Destroy(rb.gameObject, 2f);
        }
    }


}
