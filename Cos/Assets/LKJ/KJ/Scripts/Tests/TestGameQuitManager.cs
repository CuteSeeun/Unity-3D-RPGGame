using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameQuitManager : MonoBehaviour
{
    public void ReturnLogIn()
    {
        SceneManager.LoadScene("LogInScene");
    }
}
