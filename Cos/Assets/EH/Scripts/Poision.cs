using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;
public class Poision : MonoBehaviour
{
    public float poisonDuration = 10f;
    public float TikDamage = 2f;
    private bool isPoisoned = false;
    private float poisonEndTime;
        
    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Player"))
        {
            if (!isPoisoned)
            {
                //플레이어 컨트롤러에게 주체를 넘긴다
                PlayerController pc = other.gameObject.GetComponent<PlayerController>();
                //해당 스크립트를 실행하는 주체는 플레이어고 코루틴을 실행한다.
                pc.StartCoroutine(ApplyPoison());
            }
        }
    }
    IEnumerator ApplyPoison()
    {
        isPoisoned = true;
        poisonEndTime = Time.time + poisonDuration;

        // 독이 끝나는 시간동안 1초마다 데미지를 준다. 
        while(Time.deltaTime < poisonEndTime)
        {
            yield return new WaitForSeconds(1f);

            yield return null;
        }

        isPoisoned=false;
    }
   
    //버튼을 활성화하면 독 클래스를 끈다
    
    
}
