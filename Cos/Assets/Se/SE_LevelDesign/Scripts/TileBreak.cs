using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Animator tile;
    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tile.SetBool("isOn", true);
         }
    }

    //private void OnTriggerExit(Collider other)
    //{
        
    //}
    // Update is called once per frame
    void Update()
    {
        
    }
}
