using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkingHeadSystem : MonoBehaviour
{
    [Header("talking head")]
    /* talking head ui */
    public GameObject talkingHead;
    /* talking head text */
    public TMP_Text talkingHeadText;
    [Header("sectionid")]
    public int sectionID;

    void Start()
    {
        talkingHead.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("충돌?");
            switch (sectionID)
            {
                case 1:
                    talkingHeadText.text = "첫 번째 방";
                    break;
                case 2:
                    talkingHeadText.text = "두 번째 방";
                    break;
            }
            Debug.Log("코루틴 불러옴");
            ShowTalkingHeadUI();
        }
    }
    
    /* TalkingHead 보여줌 */
    public void ShowTalkingHeadUI()
    {
        Debug.Log("UI 시작");
        talkingHead.SetActive(true);
        StartCoroutine(TalkingHeadTimer(3));
    }

    /* x 초 뒤에 TalkingHeadUI 사라짐 */
    IEnumerator TalkingHeadTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideTalkingHeadUI();
    }

    /* TalkingHead 사라짐. */
    public void HideTalkingHeadUI()
    {
        talkingHead.SetActive(false);
    }
}
