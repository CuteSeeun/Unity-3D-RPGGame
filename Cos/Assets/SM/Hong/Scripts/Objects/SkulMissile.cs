using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

public class SkulMissile : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 10f;
    public GameObject explosion;
    private float attackDamage = 5;
    SFX_Manager sound;

    void Start()
    {
        sound = FindObjectOfType<SFX_Manager>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.collider.TryGetComponent(out IHp iHp))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            iHp.Hit(attackDamage, true, transform.rotation);
            sound.VFX(42);
        }
    }
}
