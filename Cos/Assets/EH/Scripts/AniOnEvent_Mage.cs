using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniOnEvent_Mage : MonoBehaviour
{
    public GameObject spell_Shoot;
    public GameObject spell_Casting;
    public GameObject spell_Raise;


    public Transform player; // 플레이어의 Transform 정보
    public float followSpeed = 5f; // 이동 속도
    public float delay = 1f;
    public Vector3 offset = Vector3.zero;

    void Update()
    {
        //플레이어 방향으로 이동
        transform.position = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
    }
    public void Spell1()
    {
        StartCoroutine(spell1());
    }

    //형준아 여러가지 실험해봤는데 지금 문제점이 Spell2 의 애니메이션동안 사운드가 진행되는데 사운드가 다 끝나기도 전에 애니메이션이 끝나서 사운드가 중간에 끊키고 Spell3의 사운드(딱!소리) 가 실행되는것같아 그래서 터지는 이펙트랑 싱크가 안맞아
    public void Spell2()
    {
        GameObject obj = Instantiate(spell_Casting, transform.position, Quaternion.identity);
        obj.AddComponent<AniOnEvent_Mage>().player = transform; // 스크립트를 추가하고 플레이어를 자신으로 설정
        SFX_Manager.Instance.VFX("lightcharge");
        Destroy(obj, 3f);
    }

    public void Spell3()
    {

        StartCoroutine(spell3());
        SFX_Manager.Instance.VFX("explosion");
    }
    
    IEnumerator spell1()
    {

        Vector3 characterForward = transform.forward;
        GameObject obj = Instantiate(spell_Shoot, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * 0.5f;
        obj.transform.position += offset;
        Quaternion effectRotation = Quaternion.LookRotation(characterForward);
        obj.transform.rotation = effectRotation;
        obj.transform.Rotate(Vector3.up, -90f, Space.Self);
        obj.SetActive(true);
        SFX_Manager.Instance.VFX("magicmissile");


        yield return new WaitForSeconds(delay);
        Destroy(obj);

    }
    IEnumerator spell3()
    {
        GameObject obj = Instantiate(spell_Raise, transform.position, Quaternion.identity);
        obj.AddComponent<AniOnEvent_Mage>().player = transform; // 스크립트를 추가하고 플레이어를 자신으로 설정
        SFX_Manager.Instance.VFX("explosion");
        obj.transform.position += Vector3.up * 0.5f;
        obj.transform.position += offset;

        yield return new WaitForSeconds(delay);
        Destroy(obj, 4f);  
        
    }

}