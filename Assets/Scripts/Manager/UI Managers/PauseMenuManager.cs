// 파일 이름: PauseMenuManager.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI 패널 연결")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject tutorial;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        // if (tutorial != null) tutorial.SetActive(false); // Awake() 시점에서 Tutorial 창은 스스로 비활성화 함
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
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    public void OnClick_Options()
    {
        optionsPanel.SetActive(true);
        // pauseMenuPanel.SetActive(false);
    }

    // 튜토리얼 창의 비활성화는 Tutorial에서 결정
    public void OnClick_Tutorial()
    {
        tutorial.SetActive(true);
    }

    public void OnClick_GoToTitle()
    {
        Time.timeScale = 1f;
        GameManager.instance.GoToTitle();
    }

    // Option 창을 닫는 버튼은 Option에 할당되어 있으므로 이곳에서 관리하지 않음
    // public void CloseOptions()
    // {
    //     optionsPanel.SetActive(false);
    //     pauseMenuPanel.SetActive(true);
    // }
}