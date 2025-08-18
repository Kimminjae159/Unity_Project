using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Manager 할당")]
    [Tooltip("이 트리거가 실행시킬 ScriptManager를 할당하세요.")]
    public DialogueManager dialogueManager;

    [Header("실행시킬 Dialogue 에셋")]
    public DialogueAsset dialogue;

    [Header("상호작용 에셋 지정 (DialogueUI -> Interaction_Text)")]
    public Button interactionText = null;

    [Header("트리거 재사용 여부 결정")]
    [Tooltip("트리거가 여러번 실행되도록 하려면 체크하세요.")]
    public bool triggerReusing = true;

    [Header("상호작용 모드(상호작용 버튼 띄우기) 활성화 여부")]
    [Tooltip("상호작용 버튼이 뜨도록 하려면 체크하세요. 체크하지 않을 경우 자동으로 Trigger가 발동됩니다.")]
    public bool interactMode = false;

    private bool isPlayerInRange = false;   // 플레이어가 Trigger 내부로 들어왔는가?
    private bool isDialogueRunning = false;  // 플레이어가 Dialogue를 실행중인가?
    private bool hasTriggered = false;      // Dialogue가 일회용일때 사용될 변수

    void Awake()
    {
        interactionText.gameObject.SetActive(false);
    }
    void Update()
    {
        // 범위내에 들어왔고, 상호작용 모드이며, 키를 눌렀을 경우에 발동
        if (isPlayerInRange && interactMode && Input.GetKeyDown(KeyCode.F))
        {
            StartDialogueController();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 "Player" 태그를 가졌는지 확인
        if (other.CompareTag("Player"))
        {
            // 상호작용 모드 아님 = 없이 무조건 발동인 경우
            if (!interactMode)
            {
                Debug.Log("OnTriggerEnter / !interactMode");
                StartDialogueController();
            }
            else
            // 상호작용 모드일 경우
            {
                isPlayerInRange = true;
                interactionText.gameObject.SetActive(true); // 상호작용키 생성
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactMode) interactionText.gameObject.SetActive(false); // 벗어나면 상호작용키 제거
        }
    }

    private void HandleDialogueEnd()
    {
        isDialogueRunning = false;
        if (interactMode) interactionText.gameObject.SetActive(true); // 상호작용키 생성
    }

    // Dialogue UI의 Interaction_Button의 OnClick()과 연결
    public void ClickOnInteraction()
    {
        StartDialogueController();
    }

    void StartDialogueController()
    {
        Debug.Log("StartDialogueController");
        if (isDialogueRunning) return; // Dialogue가 실행중인데 호출되면 반환
        if (interactMode) interactionText.gameObject.SetActive(false); // 대화창 생성되므로 상호작용키 제거

        // 아직 실행된 적 없거나, 여러 번 실행 가능한 경우에는 Dialogue 호출
        if (!hasTriggered || triggerReusing)
        {
            Debug.Log("Call StartDialogue");
            // 할당된 ScriptManager의 대화 시작 함수를 호출 + 콜백함수를 인수로 전달
            dialogueManager.StartDialogue(dialogue, HandleDialogueEnd);
            hasTriggered = true; // 실행되었다고 표시
            isDialogueRunning = true; // 실행중임을 명시
        }
    }
}