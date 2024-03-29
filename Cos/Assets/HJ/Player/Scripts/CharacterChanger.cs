using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;
    public GameObject Character4;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Character2.SetActive(false);
            Character3.SetActive(false);
            Character4.SetActive(false);
            Character1.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Character1.SetActive(false);
            Character3.SetActive(false);
            Character4.SetActive(false);
            Character2.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Character1.SetActive(false);
            Character2.SetActive(false);
            Character4.SetActive(false);
            Character3.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Character1.SetActive(false);
            Character2.SetActive(false);
            Character3.SetActive(false);
            Character4.SetActive(true);
        }
    }
}
