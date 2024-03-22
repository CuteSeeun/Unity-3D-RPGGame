using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHurt : MonoBehaviour
{
    public int Wall_Hp = 20;
    public Animator anim;
    public Vector3 offset_pos = Vector3.zero;
    public GameObject WallBroken;
    //플레이어에게 맞으면 해당 코루틴을 실행한다(피격 이펙트) 
    /*IEnumerator Wall_gethit()
    {
        int countTime = 0;

        while(countTime < 10)
        {
            //반짝반짝 거리게 alpha 값을 조정해준다.
            if (countTime % 2 == 0)
                SpriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                SpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
        SpriteRenderer.color = new Color32(255, 255, 255, 255);

        isWall_gethit = false;

        yield return null;
    
    }*/




    //플레이어 공격에 의해 Wall_Hp가 0 아래로 내려가면 해당 코드를 실행한다
    /*private void OnCollisionEnter(Collision collision)
    {
        // Wall_Hp -= 20;

        if (Wall_Hp <= 0)
        {         
            //벽이 쓰러지는 애니메이션을 시작한다.
             if(anim != null)
            {
                anim.SetTrigger("isWallDead");
            }
            //벽에 달려있는 이펙트의 위치를 조정
            GameObject obj = Instantiate<GameObject>(WallBroken, transform.position + offset_pos, transform.rotation);
            //꺼져있는 상태의 이펙트를 복사해 true로 켜준다.
            obj.SetActive(true);
            //오브젝트 삭제를 불러와 1초후 실행한다.
            Invoke("DestroyWall", 1f);
        }
        
    }
    private void DestroyWall()
    {
        Destroy(this.gameObject);
    }*/
}
