using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTest : MonoBehaviour
{
    public bool isTel;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTel)
        {
            isTel = false;
            Vector3 tel = player.position - player.forward * 2f;
            transform.position = tel;
        }
    }
}
