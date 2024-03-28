using System.Collections;
using System.Collections.Generic;
using HJ;
using UnityEngine;
using UnityEngine.AI;

public class MoveTornado : MonoBehaviour
{
    public float speed = 5f;
    Transform boss;
    NavMeshAgent agent;
    void Start()
    {
        Invoke("Move", 1.5f);
        boss = GameObject.FindWithTag("Boss").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }

    void Move()
    {
        transform.LookAt(boss.position);
        agent.SetDestination(boss.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.TryGetComponent(out IHp iHp))
        {
            iHp.Hit(5, true, Quaternion.identity);
        }
    }
}
