using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Player 태그를 가진 오브젝트가 아니면 바로 반환
        if (!other.CompareTag("Player"))
            return;

        // 현재 활성화된 씬이름을 얻는다.
        Scene nowScene = SceneManager.GetActiveScene();

        switch (nowScene.name)
        {
            case "Dongeon Enterance":
                SceneManager.LoadScene("Dongeon First");
                break;
            case "Dongeon First":
                SceneManager.LoadScene("Dongeon Enterance");
                break;

        }
    }
}