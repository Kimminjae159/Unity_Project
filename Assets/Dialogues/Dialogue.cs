using UnityEngine;

// �� ��ũ��Ʈ�� �������� 'Create' �޴��� �߰��ؼ� ����ó�� ���� �� �ְ� ��
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("��ȭ ���� (�� �پ�)")]
    [TextArea(3, 10)]
    public string[] sentences;

    [Header("������ Ȱ��ȭ ����")]
    public bool hasChoices;

    // 'hasChoices'�� true�� ���� �ν����Ϳ� ǥ�õ�
    [Header("������ ����")]
    public Choice[] choices;
}

// ������ �����͸� ���� Ŭ���� (MonoBehaviour�� ������� ����)
[System.Serializable]
public class Choice
{
    public string choiceText; // ���� ��ư�� ǥ�õ� �ؽ�Ʈ
    public string sceneToLoad;  // �̵��� ���� �̸� (����θ� �� �̵� �� ��)
}