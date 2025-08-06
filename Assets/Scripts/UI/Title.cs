using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("�� �̸�")]
    public string gameSceneName = "GameScene";

    // Start ��ư�� ����
    public void OnClickStart()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Quit ��ư�� ����
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }
}
