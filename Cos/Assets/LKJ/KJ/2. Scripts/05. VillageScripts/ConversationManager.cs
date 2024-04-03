using KJ;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvas;
    [Header("Raycast")]
    /* NPC 레이어 */
    public LayerMask npcLayer;
    /* 대화창 UI */
    public GameObject dialogueUI;
    /* Player */
    public Transform player;
    [Header("Dialogue Text")]
    /* Text 할당. */
    public TextMeshProUGUI dialogueText;
    /* 대화 큐 */
    private Queue<string> _sentences;
    /* 한 글자씩 타이핑 */
    public float typingSpeed = 0.05f;
    /* 텍스트가 한 글자씩 타이핑 중인지 확인 */
    private bool _isTyping = false;
    /* 현재 화면에 표시될 텍스트 */
    private string _currentSentence = "";
    [Header("Buttons")]
    /* 버튼들 */
    public GameObject[] choiceButtons;
    /* 텍스트 오브젝트  (분기 활성화시 텍스트만 사라지도록)*/
    public GameObject dialogueObjectText;
    /* 강화 수락 버튼 텍스트 */
    public TextMeshProUGUI enhanceText;
    /* 제작 수락 버튼 텍스트 */
    public TextMeshProUGUI craftText;
    /* 대화 UI 종료 버튼 텍스트 */
    public TextMeshProUGUI leaveText;
    [Header("Popup UI")]
    /* 팝업 UI */
    public GameObject enhanceUI;    // 강화 UI 팝업 창
    public GameObject craftUI;      // 제작 UI 팝업 창
    [Header("Interacttion Popup")]
    [Tooltip("npc 가까이 갔을 때 '대화하기' UI가 화면에 뜸, 상호작용 키 누르면 사라짐.")]
    /* npc들 */
    [SerializeField] private List<Transform> _npcs;
    /* 상호작용 범위 */
    [SerializeField] private float _interactDistance = 5.0f;
    /* UI 가 NPC 로부터 떨어진 거리 */
    [SerializeField] private float _offsetDistance = -15.5f;
    /* 상호작용 UI */
    public GameObject interactUI;
    /* 현재 상호작용UI */
    private GameObject _currentInteractUI;
    /* 가장 가까운 NPC */
    private Transform _closestNpc;
    /* 거리 간격 체크 (초) */
    public float checkInterval = 1f;
    [Header("Tag")]
    [Tooltip(" 코드의 복잡성을 피하기 위해 태그를 저장.")]
    private string _currentNpcTag;
    [Header("Bool Active Dialogue")]
    /* 대화창 활성화 상태 */
    private bool isDialogueActive = false;
    /* 현재 상호작용 중인 Npc */
    private GameObject _currentNpc;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        /* sentence 에 새로운 Queue 할당. */
        _sentences = new Queue<string>();
        /* 대화창 UI, 팝업 UI 비활성화. */
        dialogueUI.SetActive(false);
        enhanceUI.SetActive(false);
        craftUI.SetActive(false);

        /* 모든 버튼을 초기에 비활성화. */
        foreach (var button in choiceButtons)
        {
            button.SetActive(false);
        }

        StartCoroutine(CheckClosestNpcRoutine());
    }

    void Update()
    {
        #region Raycast
        /* F키(상호작용) 누르면 해당 메서드 호출. */
        if (Input.GetKeyDown(KeyCode.F))
        {

            RaycastHit hit;
            float interacitonDistance = 5.0f;
            float rayStartHeight = 1.0f;
            Vector3 rayStartPoint = player.position + Vector3.up * rayStartHeight;

            Debug.Log("Raycast 실행 : " + player.position + "방향 : " + player.forward);
            if (Physics.Raycast(rayStartPoint, player.forward, out hit, interacitonDistance, npcLayer))
            {
                if (isDialogueActive)
                {
                    return;
                }

                _currentNpcTag = string.Empty;

                foreach (var button in choiceButtons)
                {
                    button.SetActive(false);
                }
                dialogueObjectText.SetActive(true);
                /* 대화창이 켜지면 상호작용UI는 비활성화 */
                Debug.Log("대화창이 켜짐 / 상호작용UI 비활성화");
                _currentInteractUI.SetActive(false);

                /* 감지된 npc 객체를 npc tag 분석하여 맞는 대화 스크립트 생성. */
                InteractionWithNPC(hit.collider.gameObject);

                if (_currentNpc == null || _currentNpc != hit.collider.gameObject)
                {
                    Debug.Log("상호 작용 성공, 충돌한 오브젝트 :" + hit.collider.name);
                    /* 다른 NPC 와의 새로운 대화 시작. */
                    Debug.Log("초기화!");
                    _currentNpc = hit.collider.gameObject;
                    StartNewDialogue(hit.collider.gameObject);
                }
            }
        }
        #endregion
        #region 대화 텍스트
        /* Enter 키 누르면 텍스트 전부다 나오기, 다 나오면 다음 텍스트로 넘어가기. */

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_isTyping)
            {
                CompleteSentence();
            }
            else
            {
                DisplayNextSentence();
            }
        }
        #endregion

    }
    #region 대화 텍스트

    public void InteractionWithNPC(GameObject npc)
    {

        _currentNpcTag = npc.tag;
        /* npc 태그에 따라 다른 대화 출력. */
        switch (_currentNpcTag)
        {
            /* 강화 NPC */
            case "Enhance":
                StartDialogue(_currentNpcTag, new string[]
                {
                    "어서오게!",
                    "요즘 탑의 상태는 어떤가?",
                    "이러다가 우리 마을도 위험할까봐 걱정이네...",
                    "아차차, 말이 너무 길었구만, 그래 장비를 강화하러 왔는가?",
                    ""
                });
                break;
            /* 제작 NPC */
            case "Craft":
                StartDialogue(_currentNpcTag, new string[]
                {
                    "어서...오세요......",
                    "...........",
                    "...........",
                    "제작은...음..이쪽...으로...",
                    ""
                });
                break;
        }
    }

    /* 대화 내용을 화면에 표시. */
    public void StartDialogue(string npcTag, string[] dialogueLine)
    {
        /* 대화 시작시 NPC Tag 저장. */
        _currentNpcTag = npcTag;

        _sentences.Clear();

        foreach (string sentence in dialogueLine)
        {
            _sentences.Enqueue(sentence);
        }
        Debug.Log("_sentences : " + _sentences.Count);
        DisplayNextSentence();
        dialogueUI.SetActive(true);
    }

    /* 다음 대화 표시 */
    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentSentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(_currentSentence));
    }

    /* 타이핑 상태 결정 */
    IEnumerator TypeSentence(string sentence)
    {
        yield return null;
        _isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;

        yield break;
    }

    /* 타이핑 중이면 Enter 키 누르면 즉시 문장 완성. */
    public void CompleteSentence()
    {
        /* 현재 진행중인 코루틴 멈춤. */
        StopAllCoroutines();
        /* 현재 문장 즉시 완성. */
        dialogueText.text = _currentSentence;
        /* 타이핑 false 로 설정. */
        _isTyping = false;
    }

    /* 대화 상태 */
    void StartNewDialogue(GameObject npc)
    {
        /* 대화가 활성화 되어 있다면 UI 관련 상태 초기화. */
        if (isDialogueActive)
        {
            EndDialogue();
        }
        /* 현재 상호작용 중인 NPC 업데이트 */
        _currentNpc = npc;
        /* 대화 상태 활성화 */
        isDialogueActive = true;
        /* 대화 초기화 */
        Debug.Log("초기화2");
        _sentences.Clear();
        /* NPC Tag 에 따른 대화 내용 로드 및 초기화. */
        InteractionWithNPC(npc);
        /* 대화창 UI 활성화 */
        dialogueUI.SetActive(true);
    }

    /* 대화 종료 */
    public void EndDialogue()
    {
        Debug.Log("대화 끝");
        /* 대화 UI 텍스트를 비활성화. */
        dialogueObjectText.SetActive(false);
        Debug.Log("대화 텍스트 비활성화");
        /* 대화 상태 비활성화 */
        isDialogueActive = false;
        /* NPC 참조 초기화 */
        Debug.Log("NPC 참조 초기화");
        _currentNpc = null;
        /* NPC Tag 에 따라 적절한 버튼 활성화. */
        Debug.Log("호출");
        SetupInteractionButton();

    }
    #endregion
    #region 버튼
    /* NPC Tag 에 따라 버튼을 다르게 활성화. */
    public void SetupInteractionButton()
    {
        switch (_currentNpcTag)
        {
            case "Enhance":
                /* 강화 버튼 */
                Debug.Log("강화 버튼 활성화");
                choiceButtons[0].SetActive(true);
                /* 강화 버튼 텍스트 */
                enhanceText.text = "네 강화하겠습니다.";
                /* 떠나기 버튼 */
                Debug.Log("떠나기 버튼 활성화");
                choiceButtons[2].SetActive(true);
                /* 떠나기 버튼 텍스트 */
                leaveText.text = "다음에 다시 찾아뵙겠습니다.";
                break;
            case "Craft":
                /* 제작 버튼 */
                choiceButtons[1].SetActive(true);
                /* 제작 버튼 텍스트 */
                craftText.text = "아... 네.. 제작할께요.";
                /* 떠나기 버튼 */
                choiceButtons[2].SetActive(true);
                /* 떠나기 버튼 텍스트 */
                leaveText.text = " 하하..... (다음에 다시 와야겠다.)";
                break;
        }
    }

    /* 대화창 UI 종료 */
    public void ExitDialogueUI()
    {
        dialogueUI.SetActive(false);
        StartCoroutine(CheckClosestNpcRoutine());
    }

    /* 강화창 열기 */
    public void OpenEnhanceUI()
    {
        enhanceUI.SetActive(true);
        dialogueUI.SetActive(false);
    }

    /* 강화창 닫기 */
    public void CloseEnhanceUI()
    {
        enhanceUI.SetActive(false);
        StartCoroutine(CheckClosestNpcRoutine());
    }

    /* 제작창 열기 */
    public void OpenCraftUI()
    {
        craftUI.SetActive(true);
        dialogueUI.SetActive(false);
    }

    /* 제작창 닫기 */
    public void CloseCraftUI()
    {
        craftUI.SetActive(false);
        StartCoroutine(CheckClosestNpcRoutine());
    }
    #endregion
    #region 상호작용 UI
    IEnumerator CheckClosestNpcRoutine()
    {
        while (true)
        {
            float minDistance = float.MaxValue;
            Transform _closestNpc = null;
            Vector3 playerForward = player.forward;

            foreach (var npc in _npcs)
            {
                Vector3 directionToNpc = (npc.position - player.position).normalized;
                float dotProduct = Vector3.Dot(playerForward, directionToNpc);

                /* 플레이어가 대략적으로 바라보고, 상호작용 범위 내에 npc 가 있는지 */
                if (dotProduct > 0.5f)  // 1에 가까울 수록 정면이여야 함.
                {
                    float distance = Vector3.Distance(player.position, npc.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _closestNpc = npc;
                    }
                }
            }

            /* 가장 가까운 NPC가 일정 거리 이내에 있으면 UI 표시. */
            if (_closestNpc != null && minDistance <= _interactDistance)
            {
                if (_currentInteractUI == null)
                {
                    _currentInteractUI = Instantiate(interactUI);
                    /* canvas 부모로 설정. */
                    _currentInteractUI.transform.SetParent(canvas.transform, false);

                }
                /* NPC 의 스크린 좌표 위치로 UI를 이동 */
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(_closestNpc.position + Vector3.up * _offsetDistance);
                Debug.Log($"스크린 좌표값 + {screenPosition}");
                Vector2 canvasPosition;
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                /* 캔버스 좌표계로 변환 */
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out canvasPosition);

                /*캔버스 좌표계에서 추가적인 조정*/
                canvasPosition += new Vector2(1000, 200);
                Debug.Log($"캔버스 좌표값 {canvasPosition}");
                /* RectTransfrom 의 anchoredPosition 사용, screenPosition 을 RectTranform 의 좌표계로 반환. */
                /* UI 의 RectTransform 컴포넌트 가져옴. */
                _currentInteractUI.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
                /* UI 활성화 */
                Debug.Log("UI");
                _currentInteractUI.SetActive(true);

            }
            else
            {
                if (_currentInteractUI != null)
                {
                    Destroy(_currentInteractUI);
                    _currentInteractUI = null;
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
    #endregion
}
