using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int bombdamage = 5;
    public float delay = 3f;
    public Vector3 offset_pos = Vector3.zero;   

    public GameObject explosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //폭탄에 달려있는 이펙트의 위치를 조정할 수있게 offset_pos로 값을 불러와준다.
            GameObject obj = Instantiate<GameObject>(explosionEffect, transform.position + offset_pos, transform.rotation);
            //꺼져있는 상태의 이펙트를 복사해 true로 켜준다.
            obj.SetActive(true);
            //오브젝트 삭제를 불러와 1초후 실행한다.
            Invoke("DestroyObject", 1f);

            //플레이어에게 데미지 주는 코드 필요
        }
    }


    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

}
