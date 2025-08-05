// ScriptManager.cs (수정된 버전)

using System.Collections.Generic;
using UnityEngine;
using TMPro; // 레거시 UI 사용 시 필요. TextMeshPro를 사용한다면 using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("오브젝트 할당")]
    public GameObject playerObject;
    public GameObject npcObject; // NPC가 없는 독백 상황을 위해 선택 사항으로 둡니다.

    [Header("UI 할당")]
    public TextMeshProUGUI dialogueTextUI; // 대사가 표시될 UI Text. TextMeshProUGUI를 사용해도 됩니다.
    public GameObject dialoguePanel; // 대화창 전체를 묶는 패널

    [Header("스크립트 파일")]
    public TextAsset dialogueFile;

    [Header("대화 종료 후 행동")]
    [Tooltip("IDialogueAction 인터페이스를 구현한 컴포넌트를 여기에 할당하세요.")]
    public GameObject nextAction; // 인터페이스를 직접 할당할 수 없으므로 GameObject로 받고, 내부에서 형변환합니다.

    private Camera playerCam;
    private Camera npcCam;

    private string playerName;
    private string npcName;

    private List<string> dialogueLines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isInitialized = false; // 초기화가 되었는지 확인하는 플래그

    private IDialogueAction dialogueEndAction;

    // 씬이 시작될 때 대화 내용을 미리 준비만 해둡니다.
    void Start()
    {
        Initialize();
    }

    // Update는 대화가 활성화되었을 때만 동작해야 하므로 그대로 둡니다.
    void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                ShowNextLine();
            }
        }
    }

    // 초기화 함수: 컴포넌트를 찾고 파일을 파싱합니다.
    private void Initialize()
    {
        if (isInitialized) return; // 이미 초기화되었다면 실행하지 않음

        if (!ValidateComponents())
        {
            this.enabled = false;
            return;
        }

        playerCam = playerObject.GetComponentInChildren<Camera>();
        if (npcObject != null)
        {
            npcCam = npcObject.GetComponentInChildren<Camera>();
        }

        if (playerCam == null)
        {
            Debug.LogError("Player 오브젝트 또는 그 자식에게서 카메라를 찾을 수 없습니다!");
            this.enabled = false;
            return;
        }

        ParseDialogueFile();

        // 대화창 UI는 처음에 비활성화합니다.
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        dialogueTextUI.text = "";

        isInitialized = true;
    }

    /// <summary>
    /// [Public] 외부 트리거에서 이 함수를 호출하여 대화를 시작합니다.
    /// </summary>
    public void StartDialogue()
    {
        // 초기화가 안됐거나, 이미 대화가 진행 중이면 시작하지 않음
        if (!isInitialized || isDialogueActive) return;

        Debug.Log("대화를 시작합니다.");
        isDialogueActive = true;
        currentLineIndex = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        // 첫 대사를 바로 표시
        ShowNextLine();
    }

    // 파일 파싱 함수 (변경 없음)
    private void ParseDialogueFile()
    {
        dialogueLines = new List<string>(dialogueFile.text.Split('\n'));
        string firstLine = dialogueLines[0];
        string[] nameParts = firstLine.Split(',');
        foreach (var part in nameParts)
        {
            string[] definition = part.Trim().Split('=');
            if (definition[0].ToUpper() == "PLAYER")
            {
                playerName = definition[1];
            }
            else if (definition[0].ToUpper() == "NPC")
            {
                npcName = definition[1];
            }
        }
        dialogueLines.RemoveAt(0);
    }

    // 다음 대사 표시 함수 (일부 수정)
    private void ShowNextLine()
    {
        if (currentLineIndex >= dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        string line = dialogueLines[currentLineIndex].Trim();
        if (string.IsNullOrEmpty(line)) // 빈 줄은 건너뛰기
        {
            currentLineIndex++;
            ShowNextLine();
            return;
        }

        if (line.ToUpper().StartsWith("END"))
        {
            EndDialogue();
            return;
        }

        string[] parts = line.Split(new[] { ':' }, 2);
        if (parts.Length < 2)
        {
            Debug.LogWarning($"대사 형식 오류: {line}");
            currentLineIndex++;
            ShowNextLine();
            return;
        }

        string speaker = parts[0].Trim();
        string dialogue = parts[1].Trim();

        SwitchCamera(speaker);
        dialogueTextUI.text = dialogue;

        currentLineIndex++;
    }

    // 대화 종료 함수 (변경 없음)
    private void EndDialogue()
    {
        isDialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        Debug.Log("대화가 종료되었습니다.");
        if (dialogueEndAction != null)
        {
            dialogueEndAction.Apply();
        }
    }

    // 나머지 함수들(ValidateComponents, SwitchCamera)은 이전과 동일하게 유지됩니다.
    // ... (ValidateComponents, SwitchCamera 함수는 이전 코드와 동일)
    private bool ValidateComponents()
    {
        if (dialogueFile == null) { Debug.LogError("Dialogue File이 할당되지 않았습니다!"); return false; }
        if (playerObject == null) { Debug.LogError("Player Object가 할당되지 않았습니다!"); return false; }
        if (dialogueTextUI == null) { Debug.LogError("Dialogue Text UI가 할당되지 않았습니다!"); return false; }
        if (nextAction != null)
        {
            dialogueEndAction = nextAction.GetComponent<IDialogueAction>();
            if (dialogueEndAction == null) { Debug.LogError("Next Action으로 할당된 컴포넌트가 IDialogueAction 인터페이스를 구현하지 않았습니다!"); return false; }
        }
        return true;
    }
    private void SwitchCamera(string speaker)
    {
        if (speaker == playerName)
        {
            playerCam.gameObject.SetActive(true);
            if (npcCam != null) npcCam.gameObject.SetActive(false);
        }
        else if (npcObject != null && speaker == npcName)
        {
            playerCam.gameObject.SetActive(false);
            if (npcCam != null) npcCam.gameObject.SetActive(true);
        }
    }
}