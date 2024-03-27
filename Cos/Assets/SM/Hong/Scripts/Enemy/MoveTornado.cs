using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTornado : MonoBehaviour
{
    public float speed = 5f;
    Transform boss;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Move", 1.5f);
        boss = GameObject.FindWithTag("Boss").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        transform.LookAt(boss.position);
        agent.SetDestination(boss.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
