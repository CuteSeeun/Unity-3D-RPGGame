using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -Vector3.forward * Time.deltaTime * 10;

    }
}