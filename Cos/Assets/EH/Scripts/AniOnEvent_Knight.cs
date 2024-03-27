using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AniOnEvent_Knight : MonoBehaviour
{
    public GameObject _prefab_Snow_slash;
    public GameObject _prefab_stab_medium_Blue;
    public GameObject _prefab_Blocking;
  
    public GameObject _prefab_blockattack;
    //public float spawnDistance = 1f;
    public float delay = 1f;
    public float delay2 = 0.5f;

    public Vector3 offset = Vector3.zero;
    
    
    public void ATTACK1()
    {
        StartCoroutine(Attack1());
    }
    public void ATTACK2()
    {
        StartCoroutine(Attack2());

    }
    public void ATTACK3()
    {
        StartCoroutine(Attack3());

    }
    public void ATTACK4()
    {
        StartCoroutine(Attack4());

    }
    public void Block_Hit()
    {
        StartCoroutine(Block_hit());

    }
   
    public void Block_Attack()
    {
        StartCoroutine(block_attack());

    }
    IEnumerator Attack1()
    {
        //캐릭터의 현재 방향
        Vector3 characterForward = transform.forward;
        //이펙트 스노우 슬래쉬(프리펩) 생성
        GameObject obj = Instantiate(_prefab_Snow_slash, transform.position, Quaternion.identity);
        //이펙트 위치 중간으로 조정
        obj.transform.position += Vector3.up * 1.5f;
        obj.transform.position += Vector3.forward * 1f;
        //이펙트 프리팹 방향을 캐릭터 방향으로 같이 회전할 수 있게 해줌
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        //이펙트 각도 조정
        obj.transform.Rotate(Vector3.forward, -45f, Space.Self);
        //이펙트 활성화
        obj.SetActive(true);
        //SFX_Manager 에서 상속받은 인스탄스로 효과음 삽입
        SFX_Manager.Instance.VFX("Player_Knight_attack");

        //딜레이 조정 후 이펙트 삭제
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }


    IEnumerator Attack2()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Snow_slash, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1.5f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.forward, -90f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_Knight_attack");


        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }
    IEnumerator Attack3()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Snow_slash, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_Knight_attack");
        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }
    IEnumerator Attack4()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_stab_medium_Blue, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 0.5f;
        obj.transform.position += offset;//obj.transform.forward ;// * -1.2f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.up, -90f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_Knight_attack");


        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }

    IEnumerator Block_hit()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Blocking, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.forward * 1f;
        obj.transform.position += Vector3.up * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;

        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_Shield_defending");
        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }
    IEnumerator block_attack()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_blockattack, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.forward * 1f;
        obj.transform.position += Vector3.up * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;

        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_Shield_attack");
        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }

   
}
