using UnityEngine;
using UnityEngine.UI; // Button 사용을 위해 추가
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가
using TMPro;
using System.Collections;
using System;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    [Header("대화 데이터 (여기에서 필수로 할당할 필요 X)")]
    [Tooltip("외부에서 이 Controller를 호출할 때 Dialogue 에셋을 지정하지 않으면 여기에 있는 파일을 우선적으로 실행합니다.")]
    public Dialogue dialogue = null; // 이제 string[] 대신 Dialogue 파일을 통째로 받음

    [Header("UI 요소 (마우스를 올려놓아 tooltip 참고)")]
    [Tooltip("DialogueUI -> Dialogue_Panel")]
    public GameObject dialoguePanel;
    [Tooltip("DialogueUI -> Dialogue_Panel -> Viewport -> Content -> Dialogue Text")]
    public TextMeshProUGUI dialogueText;

    [Header("선택지 관련")]
    public GameObject choiceButtonPrefab; // 선택지 버튼 프리팹
    public Transform choicesLayout;       // 선택지 버튼들이 생성될 위치 (레이아웃 그룹)

    [Header("대사 출력 속도")]
    public float typingSpeed = 0.05f;

    [Header("플레이어 움직임 봉인")]
    [Tooltip("대사가 출력되는 동안 플레이어의 wasd 움직임을 봉쇄하고 싶다면 Player 오브젝트 할당")]
    public GameObject player;

    private int index = 0;
    private bool dialogueEnable = false;
    bool NextClick = false;
    bool EscapeClick = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        choicesLayout.gameObject.SetActive(false); // 선택지 레이아웃도 처음엔 꺼둠
    }

    void Update()
    {
        if (dialogueEnable)
        {
            if (NextClick || Input.GetKeyDown(KeyCode.Space))
            {
                NextClick = false;
                ContinueDialogue();
            }
            else if (EscapeClick || Input.GetKeyDown(KeyCode.Escape))
            {
                EscapeClick = false;
                // 특정 버튼 누를시 대화 및 선택지 강제 종료
                dialogueEnable = false;
                choicesLayout.gameObject.SetActive(false);
                EndDialogue();
            }
        }
    }
    // Dialogue UI의 EscapeButton의 OnClick() 동작을 이 함수에 연결
    public void ClickOnEscapeButton()
    {
        EscapeClick = true;
    }
    // Dialogue UI의 NextButton의 OnClick() 동작을 이 함수에 연결
    public void ClickOnNextButton()
    {
        NextClick = true;
    }

    // 외부에서 Diaglogue를 발동시키도록 하는 함수
    // 외부에서 보낸, 발동시킬 Dialogue와 끝났을때 실행될 콜백함수를 파라미터로 가짐
    // Dialogue를 같이 포함하지 않고 보냈다면 이 Script에 할당된 것을 우선 실행
    private Action onDialogueEndCallback;
    public void ActivateDialogue(Dialogue exDialogue = null, Action callback = null)
    {
        dialogueEnable = true;
        if (exDialogue != null)
        {
            dialogue = exDialogue;
            Debug.Log("전달받은 인자로 dialogue 업데이트");
        }

        // 콜백 함수 저장, 만약 콜백함수를 파라미터로 보내지 않으면 null이 담김
        onDialogueEndCallback = callback;

        // 대사 출력 동안에 플레이어 Move를 비활성화
        if(player) player.GetComponent<SimpleMove>().enabled = false;

        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeSentence());
    }

    void ContinueDialogue()
    {
        // 현재 문장이 타이핑 중일 때 F를 누르면, 타이핑을 즉시 완료
        if (dialogueText.text != dialogue.sentences[index])
        {
            StopAllCoroutines();
            dialogueText.text = dialogue.sentences[index];
        }
        // 타이핑이 끝난 상태에서 F를 누르면, 다음 문장으로
        else
        {
            index++;
            if (index < dialogue.sentences.Length)
            {
                StartCoroutine(TypeSentence());
            }
            else
            {
                // 모든 대사가 끝나면 선택지 표시 또는 대화 종료
                if (dialogue.hasChoices)
                {
                    ShowChoices();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        player.GetComponent<SimpleMove>().enabled = true;

        // ActivateDialogue 함수로 받은 콜백함수를 발동
        onDialogueEndCallback?.Invoke();
        onDialogueEndCallback = null;
    }

    IEnumerator TypeSentence()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue.sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void ShowChoices()
    {
        dialogueText.text = ""; // 마지막 대사는 지우거나, 질문 텍스트로 변경 가능
        choicesLayout.gameObject.SetActive(true);

        // 기존에 있던 버튼들 삭제 (다시 대화할 경우를 대비)
        foreach (Transform child in choicesLayout)
        {
            Destroy(child.gameObject);
        }

        // 데이터에 있는 선택지 만큼 버튼 생성
        foreach (Choice choice in dialogue.choices)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choicesLayout);
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    void OnChoiceSelected(Choice choice)
    {
        choicesLayout.gameObject.SetActive(false); // 선택지를 고르면 레이아웃 숨기기
        EndDialogue();

        // 씬 이동 데이터가 있다면 씬 로드
        if (!string.IsNullOrEmpty(choice.sceneToLoad))
        {
            SceneManager.LoadScene(choice.sceneToLoad);
        }
    }
}