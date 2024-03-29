using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniOnEvent_Barbarian : MonoBehaviour
{
    public GameObject _prefab_Stone_slash;
    public GameObject _prefab_stab_medium_Orange;
    public GameObject _prefab_Swinging;

    //public float spawnDistance = 1f;
    public float delay = 1f;
    public float delay2 = 0.5f;

    public Vector3 offset = Vector3.zero;

    public void SWING1()
    {
        StartCoroutine(swing1());
    }

    public void SWING2()
    {
        StartCoroutine(swing2());

    }
    public void SWING3()
    {
        StartCoroutine(swing3());

    }
    public void SWINGING()
    {
        StartCoroutine(swinging());
    }
    IEnumerator swing1()
    {
        //캐릭터의 현재 방향
        Vector3 characterForward = transform.forward;
        //이펙트 스노우 슬래쉬(프리펩) 생성
        GameObject obj = Instantiate(_prefab_Stone_slash, transform.position, Quaternion.identity);
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
        SFX_Manager.Instance.VFX("Player_barbarian_attack");

        //딜레이 조정 후 이펙트 삭제
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
    IEnumerator swing2()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Stone_slash, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;

        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_barbarian_attack");
        yield return new WaitForSeconds(delay2);
        Destroy(obj);
    }
    
    IEnumerator swing3()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_stab_medium_Orange, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 0.5f;
        obj.transform.position += offset;//obj.transform.forward ;// * -1.2f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.up, -90f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_barbarian_attack");


        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }
    IEnumerator swinging()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Swinging, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 0.5f;
        obj.transform.position += offset;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.up, -90f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Player_barbarian_spinningattack");


        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }
}
