using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f; //이동속도
    public float rotationSpeed = 3f; //회전속도

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Rotate()
    {
        float _roatate = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, _roatate * rotationSpeed * Time.fixedDeltaTime);
    }

    void Move()
    {
        float _move = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * _move * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
