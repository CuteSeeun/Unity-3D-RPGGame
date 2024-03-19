using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public GameObject owner;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * speed;
        transform.LookAt(target.transform.position);
        transform.Rotate(180, 0, 0);
    }

    void Update()
    {
        
    }
}
