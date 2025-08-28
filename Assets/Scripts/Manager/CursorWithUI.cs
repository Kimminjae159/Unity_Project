using UnityEngine;

public class CursorWithUI : MonoBehaviour
{
    // 이 UI가 활성화될 때 자동으로 호출됨
    void OnEnable()
    {
        // CursorManager에 커서를 보여달라고 요청
        cursorMng.instance.RequestCursor();
    }

    // 이 UI가 비활성화될 때 자동으로 호출됨
    void OnDisable()
    {
        // CursorManager에 커서 요청을 해제
        cursorMng.instance.ReleaseCursor();
    }
}
