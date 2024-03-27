using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinnerAnimation : MonoBehaviour
{    
    public int rotSpeed = -1;
    void Update()
    {
        transform.Rotate(0, 0, rotSpeed + Time.deltaTime);
    }
}
