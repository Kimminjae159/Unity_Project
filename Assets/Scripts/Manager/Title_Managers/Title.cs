using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("씬 이름")]
    public string gameSceneName = "GameScene";

    // Start 버튼에 연결
    public void OnClickStart()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Quit 버튼에 연결
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
