using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("옵션 창")]
    public GameObject optionPanel; // 옵션 창

    [Header("튜토리얼 창")]
    [Tooltip("(tutorial UI 상위의 오브젝트 할당)")]
    public GameObject tutorial;

    public GameObject leaderboard;


    // Start Button에 할당
    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }

    // Tutorial Button에 할당
    public void OnClickTutorial()
    {
        tutorial.SetActive(true);
    }

    public void OnClickLeaderBoard()
    {
        leaderboard.SetActive(true);
    }

    // Option Button에 할당
    public void OnClickOption()
    {
        optionPanel.SetActive(true);
    }

    // Quit Button에 할당
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
