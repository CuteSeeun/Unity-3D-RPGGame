using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    private Rigidbody myRigid;

    public int Hp = 100;
    public int Mp = 100;
    int HealEffect = 0;

    public void Died()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Healed(int HealEffect)
    {
        if (this.HealEffect > 0)
        {
            this.HealEffect += HealEffect;
        }
        else if (this.HealEffect <= 0)
        {
            this.HealEffect = HealEffect;
            StartCoroutine("Healing");
        }
    }

    IEnumerator Healing()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            this.HealEffect -= 1;
            this.Hp += 1;

            yield return new WaitForSeconds(1);
            if (this.HealEffect <= 0) break;
        }
    }



    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    //플레이어 기본적인 움직인 호리존탈과 버티칼 
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHoizontal = transform.right * _moveDirX;
        Vector3 _moveVertial = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHoizontal + _moveVertial).normalized * walkSpeed;

        //타임*델타타임을 이용해 유니티에서 속도 조절 가능
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }


}