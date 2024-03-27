using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulMissile : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 10f;
    public GameObject explosion;

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //데미지 주는 함수 호출
        }
    }
}
