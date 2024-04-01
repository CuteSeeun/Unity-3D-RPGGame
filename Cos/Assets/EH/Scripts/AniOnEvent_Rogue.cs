using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AniOnEvent_Rogue : MonoBehaviour
{
    public GameObject _prefab_Electro_slash;
    public GameObject _prefab_slash_medium_green;
    public GameObject _prefab_slash_medium_green1;
    public GameObject _prefab_stab_medium_DeepOcean;
    public float delay = 1f;
    public float delay2 = 0.5f;
    public Vector3 offset = Vector3.zero;
    public void SLASH1()
    {
        StartCoroutine(slash1_1());
        StartCoroutine(slash1_2());
    }
    public void SLASH2()
    {
        StartCoroutine(slash2_1());
        StartCoroutine(slash2_2());

    }
    public void SLASH3_1()
    {
        StartCoroutine(slash3_1());

    }
    public void SLASH3_2()
    {
        StartCoroutine(slash3_2());

    }
    
    public void USE_CROSSBOW()
    {
        StartCoroutine(use_crossbos());

    }
   
    IEnumerator slash1_1()
    {
        //캐릭터의 현재 방향
        Vector3 characterForward = transform.forward;
        //이펙트 스노우 슬래쉬(프리펩) 생성
        GameObject obj = Instantiate(_prefab_Electro_slash, transform.position, Quaternion.identity);
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
        SFX_Manager.Instance.VFX("Thief attack");

        //딜레이 조정 후 이펙트 삭제
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
    IEnumerator slash1_2()
    {
        
        Vector3 characterForward = transform.forward;
        
        GameObject obj = Instantiate(_prefab_slash_medium_green, transform.position, Quaternion.identity);
        
        obj.transform.position += Vector3.up * 1.5f;
        obj.transform.position += Vector3.forward * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        
        obj.transform.Rotate(Vector3.forward, 45f, Space.Self);
        
        obj.SetActive(true);
                        
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }


    IEnumerator slash2_1()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_stab_medium_DeepOcean, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1.5f;
        obj.transform.position += Vector3.right * -1.5f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.forward, 0f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Thief attack");


        yield return new WaitForSeconds(delay);
        Destroy(obj);

    }
    IEnumerator slash2_2()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_stab_medium_DeepOcean, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1.5f;
        obj.transform.position += Vector3.right * 1.5f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.forward, 0f, Space.Self);
        obj.SetActive(true);
        
        yield return new WaitForSeconds(delay);
        Destroy(obj);

    }
    IEnumerator slash3_1()
    {
        Vector3 characterForward = transform.forward;
        
        GameObject obj = Instantiate(_prefab_Electro_slash, transform.position, Quaternion.identity);
        
        obj.transform.position += Vector3.up * 1.5f;
        obj.transform.position += Vector3.forward * 1f;
        
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        
        obj.transform.Rotate(Vector3.forward, -45f, Space.Self);
        
        obj.SetActive(true);
        
        SFX_Manager.Instance.VFX("Thief attack");

        
        yield return new WaitForSeconds(delay);
        Destroy(obj);

    }
    IEnumerator slash3_2()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_slash_medium_green1, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 1.5f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.forward, 90f, Space.Self);
        obj.SetActive(true);
        


        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }

    IEnumerator use_crossbos()
    {
        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(_prefab_Electro_slash, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.forward * 1f;
        obj.transform.position += Vector3.up * 1f;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;

        obj.SetActive(true);
        SFX_Manager.Instance.VFX("Shield_defending");
        yield return new WaitForSeconds(delay2);
        Destroy(obj);

    }

    
}
