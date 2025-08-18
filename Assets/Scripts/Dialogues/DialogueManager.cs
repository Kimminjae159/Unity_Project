using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// DialogueAsset을 받아 대화 UI를 관리하고 대화의 흐름을 제어합니다.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 (마우스를 올려놓아 tooltip 참고)")]
    [Tooltip("DialogueUI -> Dialogue_Panel")]
    [SerializeField] private GameObject dialoguePanel;
    [Tooltip("DialogueUI -> Dialogue_Panel -> Viewport -> Content -> Dialogue Text")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choicesLayout;       // 선택지 버튼들이 생성될 위치 (레이아웃 그룹)
    [SerializeField] private GameObject choiceButtonPrefab; // 선택지 버튼 프리팹

    [Header("설정")]
    [SerializeField] private float typingSpeed = 0.05f;
    [Tooltip("대사가 출력되는 동안 플레이어의 wasd 움직임을 봉쇄하고 싶다면 Player 오브젝트 할당")]
    [SerializeField] private GameObject player; // 대화 중 움직임을 막을 플레이어 오브젝트

    // --- 내부 상태 변수 ---
    private bool dialogueEnable = false; // 대화 활성화 여부 추적
    private bool NextClick = false;      // Dialogue 창에서 버튼
    private bool EscapeClick = false;    // Dialogue 창에서 버튼 2
    private bool ChoicesBtnClick = false; // 선택지가 떴을 때 버튼을 눌렀는지 여부를 확인, 다음 Dialogue로 넘어가기 위함

    private DialogueAsset dialogueAsset;  // 외부 Dialogue Asset을 저장함
    private int dialoguePieceIndex;
    private Action onDialogueEndCallback;
    private Coroutine typingCoroutine;    // 코루틴이 진행중인지에 대한 여부를 해당 변수 할당 여부를 통해 판단
    private List<GameObject> activeChoiceButtons = new List<GameObject>(); // 버튼 클릭시 실행할 행동을 이것으로 선정

    void Start()
    {
        dialoguePanel.SetActive(false);
        choicesLayout.gameObject.SetActive(false); // 선택지 레이아웃도 처음엔 꺼둠
    }

    void Update()
    {
        if (!dialogueEnable) return; // dialogue가 사용가능 상태가 아니면 바로 해당 프레임의 Update 활동 종료
        
        if (NextClick || Input.GetKeyDown(KeyCode.Space))
        {
            NextClick = false;
            ContinueDialogue();
        }
        else if (EscapeClick || Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeClick = false;
            // 특정 버튼 누를시 대화 및 선택지 강제 종료
            choicesLayout.gameObject.SetActive(false);
            EndDialogue();
        }
        
    }

    /// <summary>
    /// 외부에서 대화를 시작하기 위해 호출하는 메인 함수입니다.
    /// </summary>
    /// <param name="dialogueAsset">시작할 Dialogue Asset</param>
    /// <param name="callback">대화가 모두 끝났을 때 실행될 함수</param>
    public void StartDialogue(DialogueAsset dialogueAsset, Action callback = null)
    {
        // 이미 대화가 진행 중이면 중복 실행 방지
        if (dialoguePanel.activeSelf)
        {
            Debug.LogWarning("이미 대화가 진행중입니다.");
            return;
        }

        this.dialogueAsset = dialogueAsset;
        onDialogueEndCallback = callback;

        // 대화 시작 준비
        dialoguePanel.SetActive(true);
        dialoguePieceIndex = -1; // -1에서 시작하여 첫 ContinueDialogue 호출 시 0이 되도록 함
        dialogueEnable = true;

        // 대사 출력 동안에 플레이어 Move를 비활성화
        if (player) player.GetComponent<SimpleMove>().enabled = false;

        // 대화 흐름 시작
        ContinueDialogue();
    }

    /// <summary>
    /// 다음 대화 조각으로 진행
    /// Trigger 발동, 사용자의 입력으로 호출됨.
    /// typingCoroutine이 할당 중(=텍스트 출력중)에 호출되면 해당 호출은 모든 text를 출력하는데 사용됨
    /// Piece에 선택지가 존재하면 OnClick() 발동으로 Index가 늘어나 다음 Piece로 넘어가지 않는 이상 아무리 호출되어도 다음으로 이동되지 않음
    /// </summary>
    public void ContinueDialogue()
    {
        // 타이핑 중일 경우, 타이핑을 즉시 완료
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            dialogueText.text = dialogueAsset.dialoguePieces[dialoguePieceIndex].sentence;
            return;
        }

        // 현재 대화 조각에 선택지가 있을 때 ContinueDialogue가 호출되면 반려(=return)함
        if (dialoguePieceIndex >= 0 && dialogueAsset.dialoguePieces[dialoguePieceIndex].choices.Length > 0)
        {
            if (ChoicesBtnClick) // 그러나 만약 선택지를 이전 프레임에서 클릭했을 경우 다음으로 이동
                ChoicesBtnClick = false;
            else
                return;
        }

        dialoguePieceIndex++;

        // 모든 대화 조각을 다 보여줬다면 대화 종료
        if (dialoguePieceIndex >= dialogueAsset.dialoguePieces.Length)
        {
            EndDialogue();
            return;
        }

        // 현재 대화 조각을 가져와서 처리
        ProcessDialoguePiece(dialogueAsset.dialoguePieces[dialoguePieceIndex]);
    }

    /// <summary>
    /// 하나의 대화 조각을 UI에 표시합니다.
    /// </summary>
    private void ProcessDialoguePiece(DialoguePiece piece)
    {
        // 이전 선택지 버튼들 제거
        ClearChoices();

        // 대사 처리
        // 만약 Sentence - 대사가 존재하면 코루틴을 할당시켜 출력하도록 함
        if (!string.IsNullOrEmpty(piece.sentence))
        {
            typingCoroutine = StartCoroutine(TypeSentence(piece.sentence));
        }
        else
        {
            dialogueText.text = ""; // 대사가 없으면 텍스트 창 비우기
        }

        // 선택지 처리
        if (piece.choices.Length > 0)
        {
            ShowChoices(piece.choices);
        }
    }

    /// <summary>
    /// 대사를 타이핑 효과와 함께 출력하는 코루틴입니다.
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null; // 타이핑 완료
    }

    /// <summary>
    /// 선택지 버튼들을 생성하고 UI에 표시합니다.
    /// </summary>
    private void ShowChoices(Choice[] choices)
    {
        choicesLayout.gameObject.SetActive(true); // 버튼 생성 Layout 활성화

        foreach (Choice choice in choices)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choicesLayout); // 버튼 생성 Transform 위치에 맞춰 버튼 인스턴트 생성
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText; // 생성한 버튼에 문자 할당

            Button button = buttonGO.GetComponent<Button>();
            // 버튼 클릭 시, 해당 choice에 연결된 UnityEvent를 실행
            button.onClick.AddListener(() => { OnChoiceSelected(choice); });

            activeChoiceButtons.Add(buttonGO);  // 생성한 버튼 인스턴트 할당, 이후 ClearChoices에서 버튼을 제거하는데 활용
        }
    }

    /// <summary>
    /// 선택지가 클릭되었을 때 호출되어, 해당 선택지의 데이터를 해석하고 실행합니다.
    /// 선택지에 따른 특정 동작이 추가될 때마다 해당 함수를 수정해야 합니다.
    /// </summary>
    /// <param name="choice">플레이어가 선택한 Choice 데이터</param>
    private void OnChoiceSelected(Choice choice)
    {
        // 1. 공감수치 변경
        if (choice.sympathyChange != 0)
        {
            GameManager.instance.sympathyValue += choice.sympathyChange;
            // sympathyUI.UpdateUI(GameManager.Instance.sympathyValue); // UI 갱신
        }

        // 2. 점수 변경
        if (choice.scoreChange != 0)
        {
            GameManager.instance.score += choice.scoreChange;
            // scoreUI.UpdateUI(GameManager.Instance.score); // UI 갱신
        }

        // 3. 씬 변경
        if (!string.IsNullOrEmpty(choice.sceneToLoad))
        {
            EndDialogue(); // 씬 전환 전 대화는 무조건 종료
            // 씬 전환 로직 실행
            SceneManager.LoadScene(choice.sceneToLoad);
            return;
        }

        // 4. 다른 Dialogue로 교체
        if (choice.nextDialogue != null)
        {
            // 이어지는 대화가 있다면, 현재 대화 정보를 교체하고 바로 시작
            dialogueAsset = choice.nextDialogue;
            dialoguePieceIndex = -1;
            ContinueDialogue();
            return;
        }

        // 이어지는 대화가 없다면, 현재 대화의 다음 조각으로 넘어감
        ChoicesBtnClick = true;
        ContinueDialogue();
        
    }

    /// <summary>
    /// 화면에 표시된 선택지 버튼들을 모두 제거합니다.
    /// </summary>
    private void ClearChoices()
    {
        foreach (GameObject button in activeChoiceButtons)
        {
            Destroy(button);
        }
        activeChoiceButtons.Clear();
        choicesLayout.gameObject.SetActive(false);
    }

    /// <summary>
    /// 대화를 종료하고 관련 리소스를 정리합니다.
    /// </summary>
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Dialogue 패널 제거
        ClearChoices();  // 선택지 제거
        if (player) player.GetComponent<SimpleMove>().enabled = true;   // 플레이어 무브 가능

        // 저장해둔 콜백 함수 실행
        onDialogueEndCallback?.Invoke();

        // 내부 상태 초기화
        dialogueEnable = false;
        dialogueAsset = null;
        onDialogueEndCallback = null;
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
}
