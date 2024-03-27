using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public GameObject owner;
    public GameObject effect;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            //플레이어어게 데미지를 주는 함수 호출
            Destroy(gameObject);
        }
    }
}
