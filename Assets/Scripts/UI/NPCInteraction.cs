using UnityEngine;
using UnityEngine.UI; // Button ����� ���� �߰�
using UnityEngine.SceneManagement; // �� ������ ���� �߰�
using TMPro;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    [Header("��ȭ ������")]
    public Dialogue dialogue; // ���� string[] ��� Dialogue ������ ��°�� ����

    [Header("UI ���")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI interactionText;

    [Header("������ ����")]
    public GameObject choiceButtonPrefab; // ������ ��ư ������
    public Transform choicesLayout;       // ������ ��ư���� ������ ��ġ (���̾ƿ� �׷�)

    private int index = 0;
    private bool isPlayerInRange = false;
    private bool isTalking = false;
    public float typingSpeed = 0.05f;

    void Start()
    {
        dialoguePanel.SetActive(false);
        interactionText.gameObject.SetActive(false);
        choicesLayout.gameObject.SetActive(false); // ������ ���̾ƿ��� ó���� ����
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
        // ���� ������ Ÿ���� ���� �� F�� ������, Ÿ������ ��� �Ϸ�
        if (dialogueText.text != dialogue.sentences[index])
        {
            StopAllCoroutines();
            dialogueText.text = dialogue.sentences[index];
        }
        // Ÿ������ ���� ���¿��� F�� ������, ���� ��������
        else
        {
            index++;
            if (index < dialogue.sentences.Length)
            {
                StartCoroutine(TypeSentence());
            }
            else
            {
                // ��� ��簡 ������ ������ ǥ�� �Ǵ� ��ȭ ����
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
        dialogueText.text = ""; // ������ ���� ����ų�, ���� �ؽ�Ʈ�� ���� ����
        choicesLayout.gameObject.SetActive(true);

        // ������ �ִ� ��ư�� ���� (�ٽ� ��ȭ�� ��츦 ���)
        foreach (Transform child in choicesLayout)
        {
            Destroy(child.gameObject);
        }

        // �����Ϳ� �ִ� ������ ��ŭ ��ư ����
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
        choicesLayout.gameObject.SetActive(false); // �������� ���� ���̾ƿ� �����
        EndDialogue();

        // �� �̵� �����Ͱ� �ִٸ� �� �ε�
        if (!string.IsNullOrEmpty(choice.sceneToLoad))
        {
            SceneManager.LoadScene(choice.sceneToLoad);
        }
    }

    // �÷��̾� ���� ���� (������ ����)
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
            // ������ ����� ��ȭ �� ������ ���� ����
            choicesLayout.gameObject.SetActive(false);
            EndDialogue();
        }
    }
}