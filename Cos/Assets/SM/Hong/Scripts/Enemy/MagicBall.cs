using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

/// <summary>
/// MageEnemy가 발사하는 MagicBall 스크립트
/// 플레이어를 주어준 속도로 추격하고 충돌 시 데미지 부여
/// </summary>
public class MagicBall : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public GameObject owner;
    public GameObject effect;
    Rigidbody rb;
    private float attackDamage = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * speed;
        Invoke("del", 5);
    }

    void Update()
    {
        rb.velocity = (target.transform.position - transform.position).normalized * speed;
        transform.LookAt(target.transform.position);
    }

    public void del()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out IHp iHp))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            //플레이어어게 데미지를 주는 함수 호출
            iHp.Hit(attackDamage, false, transform.rotation);
            Destroy(gameObject);
        }
    }
}
