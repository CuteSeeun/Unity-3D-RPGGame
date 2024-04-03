using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInteraction : MonoBehaviour
{
    [Header("InteractionUI")]
    /* 플레이어 위치에 고정 */
    private Transform player;
    /* 상호작용 UI */
    public GameObject interactUI;
    /* UI 플레이어로부터 얼마만큼 올릴건지 */
    public float moveUI = 5.0f;
    /* 상호작용 범위 */
    public float interactRange = 3.0f;
    /* 상호작용 리스트 */
    public List<GameObject> interactionObjects;

    void Start()
    {
        Debug.Log("비활성화");
        interactUI.SetActive(false);
        player = GameObject.FindWithTag("Player").transform;
    }

 
    void Update()
    {
        /* 가장 가까운 상호작용 가능 오브젝트 찾기 */
        float closestDistance = float.MaxValue; // 가장 가까운 오브젝트와의 거리를 저장할 변수를 최대값으로 초기화.
        foreach (var interactionObject in interactionObjects)   // 상호작용 가능 오브젝트 순회
        {
            float distance = Vector3.Distance(player.position, interactionObject.transform.position);   // 플레이어와 오브젝트 사이의 거리를 변수로 저장.
            if (distance < closestDistance)// 현재 오브젝트가 지금까지 찾은 가장 가까운 오브젝트보다 가깝다면
            {
                Debug.Log("업데이트");
                closestDistance = distance; // 가장 가까운 거리를 현재 거리로 업데이트.
            }
        }
        if (closestDistance <= interactRange)   // 플레이어와 가장 가까운 오브젝트가 상호작용 범위에 있으면
        {
            Debug.Log("UI 활성화");
            /* 플레이어가 collider 에 진입하면 활성화. */
            ShowInteractionUI();
        }
        else   // 없으면
        {
            Debug.Log("UI 비활성화");
            /* 플레이어가 collider 에 벗어나면 비활성화. */
            HideInteractionUI();
        }
    }

    /* UI 활성화 */
    public void ShowInteractionUI()
    {
        /* UI 활성화 */
        interactUI.SetActive(true);
        /* UI 위치 */
        interactUI.transform.position = player.position + Vector3.up * moveUI;
        /* UI 를 카메라로부터 항상 정면으로 보이게 설정. */
        //interactUI.transform.LookAt(Camera.main.transform);
        Vector3 direction = Camera.main.transform.position - interactUI.transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        interactUI.transform.rotation = Quaternion.Slerp(interactUI.transform.rotation, rotation, Time.deltaTime * 10);
    }

    /* UI 비활성화 */
    public void HideInteractionUI()
    {
        interactUI.SetActive(false);
    }
}
