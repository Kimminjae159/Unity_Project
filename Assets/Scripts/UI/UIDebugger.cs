using UnityEngine;

public class UIDebugger : MonoBehaviour
{
    // 유니티 에디터에서 연결할 UI들의 RectTransform
    public RectTransform contentRect;
    public RectTransform dialogueTextRect;

    void Update()
    {
        // 매 프레임마다 Console 창에 너비 값을 출력
        if (contentRect != null && dialogueTextRect != null)
        {
            // dialogueTextRect.gameObject.activeInHierarchy는 텍스트 오브젝트가 활성화 상태일 때만 로그를 찍기 위함입니다.
            if (dialogueTextRect.gameObject.activeInHierarchy)
            {
                Debug.Log($"Content Width: {contentRect.rect.width}  |  Dialogue_Text Width: {dialogueTextRect.rect.width}");
            }
        }
    }
}