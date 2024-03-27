using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Portion : MonoBehaviour
{
    public GameObject DoorLock;
    public GameObject Portion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DoorLock.SetActive(false);
                Destroy(Portion);
            }
        }
    }
}
