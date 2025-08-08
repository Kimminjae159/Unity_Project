using UnityEngine;

// 이 스크립트를 에디터의 'Create' 메뉴에 추가해서 파일처럼 만들 수 있게 함
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("대화 내용 (한 줄씩)")]
    [TextArea(3, 10)]
    public string[] sentences;

    [Header("선택지 활성화 여부")]
    public bool hasChoices;

    // 'hasChoices'가 true일 때만 인스펙터에 표시됨
    [Header("선택지 내용")]
    public Choice[] choices;
}

// 선택지 데이터를 담을 클래스 (MonoBehaviour를 상속하지 않음)
[System.Serializable]
public class Choice
{
    public string choiceText; // 선택 버튼에 표시될 텍스트
    public string sceneToLoad;  // 이동할 씬의 이름 (비워두면 씬 이동 안 함)
}