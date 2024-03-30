using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    /* 알림 메시지 */
    [Header("Notification")]
    public GameObject notification;
    public TMP_Text notificationText;
    // 현재 아이템 
    // 아이템 수집 로직 -> switch Type 별로 나누어도 됨.

    void Start()
    {
        
    }

    public void CollectItem(string item)
    {
        // 아이템을 인벤토리에 추가하는 로직 넣기.

        /* 알림메시지 표시 */
        notification.SetActive(true);
        notificationText.text = $"{item} +1"; // $"{getItem}을 얻었습니다."

        StartCoroutine(HideNotification(3));
    }

    /* 알림 메시지 자동 숨김. */
    IEnumerator HideNotification(float delay)
    {
        yield return new WaitForSeconds(delay);
        notification.SetActive(false);
    }
}
