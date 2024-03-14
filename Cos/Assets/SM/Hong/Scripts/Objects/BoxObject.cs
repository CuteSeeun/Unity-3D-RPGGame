using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    private float hp = 1;
    public GameObject potion;
    public GameObject item;
    void Start()
    {
        
    }

    void Update()
    {
        if(hp < 0)
        {
            ItemSpawn();
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
    }
}
