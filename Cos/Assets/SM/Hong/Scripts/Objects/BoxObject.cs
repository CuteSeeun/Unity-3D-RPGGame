using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    public float hp = 1;
    bool isDestroy;
    public GameObject potion;
    public GameObject item;
    public GameObject destroyEffect;
    void Start()
    {
        
    }

    void Update()
    {
        if(hp <= 0 && !isDestroy)
        {
            isDestroy = true;
            ItemSpawn();
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
    }

    void ItemSpawn()
    {
        int spawnitem = Random.Range(0, 5);
        if(spawnitem == 0 )
        {
            Instantiate(potion, transform.position, Quaternion.identity);

        }
        else
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        Destroy();
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
