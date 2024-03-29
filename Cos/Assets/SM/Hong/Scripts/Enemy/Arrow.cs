using HJ;
using UnityEngine;

public class Arrow : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(attackDamage, false, transform.rotation);
            Destroy(gameObject);
        }
    }
}
