using UnityEngine;
using UnityEngine.SceneManagement;

public class cursorMng : MonoBehaviour
{
    public static cursorMng instance;
    private static cursorMng _instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            DestroyImmediate(this);
        }
        instance = _instance;
    }
    // ------ 커서 활성화 관리 ---------
    private int cursorRequestCount = 0;


    public void RequestCursor()
    {
        //Debug.Log($"[CursorManager] RequestCursor 호출됨! 이전 카운트: {cursorRequestCount}", this.gameObject);
        cursorRequestCount++;
        //Debug.Log($"[CursorManager] 새로운 카운트: {cursorRequestCount}", this.gameObject);

        UpdateCursorState();
    }

    /// <summary>
    /// 더 이상 커서가 필요 없을 때 호출하는 함수
    /// </summary>
    public void ReleaseCursor()
    {
        //Debug.Log($"[CursorManager] ReleaseCursor 호출됨! 이전 카운트: {cursorRequestCount}", this.gameObject);
        cursorRequestCount--;
        // 카운터가 0 미만으로 내려가지 않도록 방지
        if (cursorRequestCount < 0)
        {
            cursorRequestCount = 0;
        }
        //Debug.Log($"[CursorManager] 새로운 카운트: {cursorRequestCount}", this.gameObject);
        UpdateCursorState();
    }

    /// <summary>
    /// 현재 카운터 상태에 따라 실제 커서의 가시성과 잠금 상태를 업데이트하는 함수
    /// </summary>
    private void UpdateCursorState()
    {
        if (cursorRequestCount > 0)
        {
            // 커서 요청이 하나라도 있으면, 커서를 보이게 하고 잠금을 해제합니다.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            // 커서 요청이 하나도 없으면, 커서를 숨기고 중앙에 잠급니다 (일반적인 게임 플레이 상태).
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // [추가] 씬이 바뀔 때 카운터를 초기화해야 할 경우를 대비한 함수
    public void ResetCursor()
    {
        cursorRequestCount = 0;
        UpdateCursorState();
    }

    // 스크립트가 활성화될 때 씬 로드 이벤트에 함수를 등록
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 비활성화될 때 이벤트 등록 해제
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // 씬이 로드될 때마다 호출될 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cursorRequestCount = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void DialogueEnd()
    {
        ResetCursor();
    }
}
