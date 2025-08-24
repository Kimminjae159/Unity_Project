// 파일 이름: PauseMenuManager.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI 패널 연결")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClick_Options()
    {
        optionsPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    // ★★★ 여기가 수정된 부분입니다 ★★★
    public void OnClick_Tutorial()
    {
        Time.timeScale = 1f;

        // 떠나기 전에, 현재 씬의 이름을 SceneMemory에 저장합니다.
        SceneMemory.previousSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("Tutorial");
    }

    public void OnClick_GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title1");
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}