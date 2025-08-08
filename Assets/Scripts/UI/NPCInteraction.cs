using UnityEngine;
using UnityEngine.UI; // Button 사용을 위해 추가
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가
using TMPro;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    [Header("대화 데이터")]
    public Dialogue dialogue; // 이제 string[] 대신 Dialogue 파일을 통째로 받음

    [Header("UI 요소")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI interactionText;

    [Header("선택지 관련")]
    public GameObject choiceButtonPrefab; // 선택지 버튼 프리팹
    public Transform choicesLayout;       // 선택지 버튼들이 생성될 위치 (레이아웃 그룹)

    private int index = 0;
    private bool isPlayerInRange = false;
    private bool isTalking = false;
    public float typingSpeed = 0.05f;

    void Start()
    {
        dialoguePanel.SetActive(false);
        interactionText.gameObject.SetActive(false);
        choicesLayout.gameObject.SetActive(false); // 선택지 레이아웃도 처음엔 꺼둠
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                ContinueDialogue();
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        index = 0;
        interactionText.gameObject.SetActive(false);
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
        isTalking = false;
        dialoguePanel.SetActive(false);
        if (isPlayerInRange)
        {
            interactionText.gameObject.SetActive(true);
        }
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

    // 플레이어 감지 로직 (이전과 동일)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!isTalking)
            {
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionText.gameObject.SetActive(false);
            // 범위를 벗어나면 대화 및 선택지 강제 종료
            choicesLayout.gameObject.SetActive(false);
            EndDialogue();
        }
    }
}