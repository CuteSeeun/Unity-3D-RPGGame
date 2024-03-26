using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy1;
   // public GameObject enemy2;
    public float SpawnStop = 10;

    bool swi = true;

    public float StartTime = 3;

    public GameObject ItemLock;



    //void Update ()
    //{
    //if (!ItemLock.activeSelf)
    //{
    //StartCoroutine("RandomSpawn");
    //Invoke("Stop", SpawnStop);
    //}
    //}

    //private void OnTriggerStay(Collider other)
    //{
       // if (!ItemLock.activeSelf)
       // {
       //     Spawning();
        //}
    //}
    private void Spawning()
    {
        StartCoroutine("RandomSpawn");
        Invoke("Stop", SpawnStop);
    }

    IEnumerator RandomSpawn()
    {
        while (swi)
        {
            yield return new WaitForSeconds(StartTime);

            Instantiate(enemy1, transform.position, Quaternion.identity);
        }
    }

    void Stop()
    {
        swi = false;
    }

    
}
