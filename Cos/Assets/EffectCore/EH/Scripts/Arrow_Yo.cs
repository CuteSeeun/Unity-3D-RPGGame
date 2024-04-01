using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Yo : MonoBehaviour
{
    public Transform transform;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);

        //transform.forward
        //transform.up
        transform.position += transform.forward * Time.deltaTime * 5;
         
    }
}
