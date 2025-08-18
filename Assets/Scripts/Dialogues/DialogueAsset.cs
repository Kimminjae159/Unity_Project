using UnityEngine;
using UnityEngine.Events; // UnityEvent를 사용하기 위해 추가

/// <summary>
/// 대화의 흐름 전체를 담는 ScriptableObject입니다.
/// 대화 조각(DialoguePiece)들의 배열로 구성됩니다.
/// </summary>
[CreateAssetMenu(fileName = "New Dialogue Asset", menuName = "Dialogue System/DialogueAsset")]
public class DialogueAsset : ScriptableObject
{
    // 대화의 각 단계를 순서대로 담는 배열
    public DialoguePiece[] dialoguePieces;
}

/// <summary>
/// 대화의 한 단계를 구성하는 데이터 클래스입니다.
/// 대사와 선택지를 모두 포함할 수 있어, 다양한 조합이 가능합니다.
/// </summary>
[System.Serializable]
public class DialoguePiece
{
    [Header("대사 내용 (비워두면 대사 없이 진행)")]
    [TextArea(3, 10)]
    public string sentence;

    [Header("선택지 (없으면 다음 대사로 자동 진행)")]
    public Choice[] choices;
}

/// <summary>
/// 선택지 하나에 대한 데이터와, 선택 시 실행될 이벤트를 담는 클래스입니다.
/// </summary>
[System.Serializable]
public class Choice
{
    [Header("버튼에 표시될 텍스트")]
    public string choiceText;

    [Header("선택시 변화 혹은 실행시킬 것 (tooltip 참고)")]
    [Tooltip("해당 선택지를 고를시 다음에 이어질 대화 에셋, 비어있으면 대화 변경되지 않음")]
    public DialogueAsset nextDialogue = null;

    [Tooltip("해당 선택지를 고를시 이동할 씬의 이름, 비어있으면 씬 이동하지 않음")]
    public string sceneToLoad = null;

    [Tooltip("해당 선택지를 고를시 변화할 공감 수치 (양수/음수), 비어있거나 0이면 변화 발생 안함")]
    public int sympathyChange = 0;

    [Tooltip("해당 선택지를 고를시 변화할 점수 (양수/음수), 비어있거나 0이면 변화 발생 안함")]
    public int scoreChange = 0;

    // 여기에 필요한 다른 결과들을 데이터로 계속 추가 가능
    // public bool giveQuestItem;
}
