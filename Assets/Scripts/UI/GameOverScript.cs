using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.

public class GameOverScript : MonoBehaviour
{
    // 게임오버 UI 전체를 담고 있는 GameObject (Canvas 또는 Panel)를 인스펙터에서 연결합니다.
    public GameObject gameOverCanvas;
    public GameObject otherUI;

    // 게임이 시작될 때 게임오버 UI를 보이지 않게 설정합니다.
    void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    // Timer 스크립트에서 호출할 게임오버 함수입니다.
    public void EndingFunc()
    {
        // 게임오버 UI를 화면에 표시합니다.
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            otherUI.SetActive(false);
        }
        Debug.Log("Game Over!");
    }

    // '재시작' 버튼에 연결할 함수입니다.
    public void RestartGame()
    {
        // 현재 씬을 처음부터 다시 로드합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // '종료' 버튼에 연결할 함수입니다.
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        // 에디터에서는 동작하지 않지만, 빌드된 게임에서는 프로그램이 종료됩니다.
        SceneManager.LoadScene(0);
    }
}
