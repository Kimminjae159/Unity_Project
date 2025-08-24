using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsPanel; // 옵션 패널 UI 오브젝트를 연결할 변수

    void Start()
    {
        // 게임 시작 시 옵션 패널은 비활성화 상태로
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    // --- 버튼 클릭 이벤트에 연결할 함수들 ---

    public void OnClick_GameStart()
    {
        Debug.Log("게임 시작 버튼 클릭!");
        // "GameScene"은 실제 게임이 진행되는 씬의 이름으로 변경해야 합니다.
        SceneManager.LoadScene("level 0"); 
    }

    public void OnClick_Tutorial()
    {
        Debug.Log("튜토리얼 버튼 클릭!");
        // "TutorialScene"은 튜토리얼 씬의 이름으로 변경해야 합니다.
        SceneManager.LoadScene("Tutorial");
    }

    public void OnClick_Leaderboard()
    {
        Debug.Log("리더보드 버튼 클릭!");
        // 리더보드 UI를 활성화하거나, 리더보드 씬으로 이동
        // 예시: SceneManager.LoadScene("LeaderboardScene");
    }

    public void OnClick_Options()
    {
        Debug.Log("옵션 버튼 클릭!");
        // 옵션 패널이 있으면 활성화
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void OnClick_Exit()
    {
        Debug.Log("나가기 버튼 클릭!");
        // 에디터에서는 동작하지 않지만, 빌드된 게임에서는 종료됨
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}