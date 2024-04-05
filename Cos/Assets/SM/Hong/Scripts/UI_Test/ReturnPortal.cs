using System.Collections;
using System.Collections.Generic;
using KJ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnPortal : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("저장 완료");
        NetData.Instance.SavePlayerDB(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene("GotoVillage");
        }
    }
}
