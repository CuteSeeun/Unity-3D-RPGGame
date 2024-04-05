using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

/// <summary>
/// 보스 1페이즈가 Skul 패턴 때 소환하는 오브젝트의 스크립트
/// 해당 오브젝트가 바라보는 방향으로 지정된 속도로 이동
/// 플레이어와 출돌 시 지정된 데미지를 부여 후 파괴
/// </summary>
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
