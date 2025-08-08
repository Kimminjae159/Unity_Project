using UnityEngine;

public class UIDebugger : MonoBehaviour
{
    // ����Ƽ �����Ϳ��� ������ UI���� RectTransform
    public RectTransform contentRect;
    public RectTransform dialogueTextRect;

    void Update()
    {
        // �� �����Ӹ��� Console â�� �ʺ� ���� ���
        if (contentRect != null && dialogueTextRect != null)
        {
            // dialogueTextRect.gameObject.activeInHierarchy�� �ؽ�Ʈ ������Ʈ�� Ȱ��ȭ ������ ���� �α׸� ��� �����Դϴ�.
            if (dialogueTextRect.gameObject.activeInHierarchy)
            {
                Debug.Log($"Content Width: {contentRect.rect.width}  |  Dialogue_Text Width: {dialogueTextRect.rect.width}");
            }
        }
    }
}