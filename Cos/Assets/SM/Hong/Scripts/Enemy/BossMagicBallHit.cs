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
    /// <summary>
    /// 보스2페이즈가 소환하는 MagicBall 오브젝트가 플레이어와 충돌 시 데미지를 주는 함수
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(5, true, transform.rotation);
        }
    }
}
