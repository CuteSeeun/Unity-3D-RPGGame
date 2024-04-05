using HJ;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// 화살이 발사되는데 필요한 요소
    /// 화살의 속도, 화살의 타겟, 화살의 주인, 화살의 데미지
    /// </summary>
    public float speed;
    public GameObject target;
    public GameObject owner;
    Rigidbody rb;
    float attackDamage = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * speed;
        transform.LookAt(target.transform.position);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// IHp 인터페이스를 가지고 있는 오브젝트와 충돌시 데미지를 주는 hit함수 호출
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(attackDamage, false, transform.rotation);
            Destroy(gameObject);
        }
    }
}
