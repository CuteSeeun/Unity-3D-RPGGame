using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public GameObject owner;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * speed;
        Invoke("del", 5);
    }

    void Update()
    {
        rb.velocity = (target.transform.position - transform.position).normalized * speed;
    }

    public void del()
    {
        Destroy(gameObject);
    }
}
