using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // 게임오버 UI를 인스펙터에서 연결
    public GameObject gameOverUI;
    public GameObject buttons;

    // 게임이 시작될 때 게임오버 UI를 보이지 않게 설정하는 것은 StageManager이므로 건들이지 않음

    // 게임 오버시, 외부에서 호출할 함수
    public void EndingFunc()
    {
        // 게임오버 UI를 화면에 표시합니다.
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            buttons.SetActive(false);
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
