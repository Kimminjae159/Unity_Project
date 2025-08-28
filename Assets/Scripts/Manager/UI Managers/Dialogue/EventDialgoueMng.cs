using UnityEngine;
using TMPro;
using System.Collections;
using System;

/// <summary>
/// DialogueAsset의 문자열(Sentence)을 UI에 순차적으로, 자동으로 출력합니다.
/// 사용자 입력 없이 지정된 시간 간격에 따라 대사를 표시하고, 모든 대사가 끝나면 자동으로 UI를 비활성화합니다.
/// </summary>
public class EventDialgoueMng : MonoBehaviour
{
    [Header("UI 요소")]
    [Tooltip("대사가 표시될 TextMeshProUGUI 컴포넌트")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("설정")]
    [Tooltip("타이핑 효과의 속도 (한 글자당 대기 시간)")]
    [SerializeField] private float typingSpeed = 0.05f;
    [Tooltip("한 대사가 완료된 후 다음 대사로 넘어가기까지의 대기 시간")]
    [SerializeField] private float nextDialogueDelay = 1.5f;
    [Tooltip("모든 대화가 끝난 후 UI가 비활성화되기까지의 대기 시간")]
    [SerializeField] private float endDelay = 1.0f;

    // --- 내부 상태 변수 ---
    private Coroutine dialogueCoroutine;
    private Action onDialogueEndCallback;

    /// <summary>
    /// 컴포넌트 시작 시, Dialogue UI를 비활성화합니다.
    /// </summary>
    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 외부에서 이벤트 대화를 시작하기 위해 호출하는 메인 함수입니다.
    /// </summary>
    /// <param name="dialogueAsset">출력할 대사들이 담긴 DialogueAsset</param>
    /// <param name="callback">대화가 모두 끝났을 때 실행될 콜백 함수</param>
    public void EventDialogueStart(DialogueAsset dialogueAsset, Action callback = null)
    {
        // 이미 대화가 진행 중이면 중복 실행 방지
        if (dialogueCoroutine != null)
        {
            Debug.LogWarning("이미 이벤트 대화가 진행중입니다.");
            return;
        }

        this.onDialogueEndCallback = callback;

        // Dialogue UI를 활성화하고 대화 처리 코루틴 시작
        gameObject.SetActive(true);
        dialogueCoroutine = StartCoroutine(ProcessDialogue(dialogueAsset));
    }

    /// <summary>
    /// DialogueAsset의 모든 대사를 순차적으로 처리하는 코루틴입니다.
    /// </summary>
    private IEnumerator ProcessDialogue(DialogueAsset dialogueAsset)
    {
        // DialogueAsset에 포함된 모든 대사 조각(DialoguePiece)을 순회합니다.
        foreach (var piece in dialogueAsset.dialoguePieces)
        {
            // 대사(Sentence)가 비어있지 않은 경우에만 처리합니다.
            if (!string.IsNullOrEmpty(piece.sentence))
            {
                // 타이핑 효과 코루틴을 실행하고 끝날 때까지 대기합니다.
                yield return StartCoroutine(TypeSentence(piece.sentence));

                // 다음 대사로 넘어가기 전, 지정된 시간만큼 대기합니다.
                yield return new WaitForSeconds(nextDialogueDelay);
            }
        }

        // 모든 대사가 끝난 후, 지정된 시간만큼 대기합니다.
        yield return new WaitForSeconds(endDelay);

        // 저장해둔 콜백 함수가 있다면 실행합니다.
        onDialogueEndCallback?.Invoke();

        // Dialogue UI를 비활성화합니다.
        gameObject.SetActive(false);

        // 코루틴 상태를 초기화합니다.
        dialogueCoroutine = null;
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
    }
}
