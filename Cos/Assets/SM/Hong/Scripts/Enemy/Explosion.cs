using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;

/// <summary>
/// 보스 2페이즈 explosion 패턴 때 소환 되는 오브젝트의 코드
/// </summary>
public class Explosion : MonoBehaviour
{
    Collider col;
    /// <summary>
    /// 0.75초 후 콜라이더 생성. 콜라이더는 플레이어와 출돌 시 데미지를 부여. 1.5초 후 게임오브젝트 제거
    /// </summary>
    void Start()
    {
        Invoke("Remove", 1.5f);
        Invoke("Col", 0.75f);
        col = GetComponent<CapsuleCollider>();
        col.enabled = false;
    }

    void Update()
    {
        
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    void Col()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(5, true, Quaternion.identity);
        }
    }
}
