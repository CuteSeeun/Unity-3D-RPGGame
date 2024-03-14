using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //화살의 수명을 3초로 제한한다
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //20의 속도로 날아간다
        transform.position += -Vector3.forward * Time.deltaTime * 20;

    }
}