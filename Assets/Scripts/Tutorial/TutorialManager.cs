// 파일 이름: TutorialManager.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialPage
    {
        public Sprite image;
        [TextArea(3, 5)]
        public string description;
    }

    [Header("튜토리얼 페이지 목록")]
    [SerializeField] private TutorialPage[] pages;

    [Header("UI 연결")]
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;

    private int currentPageIndex = 0;

    void Start()
    {
        ShowPage(currentPageIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            GoToNextPage();
        }
    }

    private void ShowPage(int index)
    {
        tutorialImage.sprite = pages[index].image;
        descriptionText.text = pages[index].description;

        prevButton.SetActive(index > 0);

        if (index == pages.Length - 1)
        {
            // 마지막 페이지일 때 버튼 텍스트를 "돌아가기"로 변경
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "돌아가기";
        }
        else
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "다음";
        }
    }

    // ★★★ 여기가 수정된 부분입니다 ★★★
    public void GoToNextPage()
    {
        // 마지막 페이지라면
        if (currentPageIndex >= pages.Length - 1)
        {
            // SceneMemory에 저장된 씬 이름이 있는지 확인합니다.
            if (!string.IsNullOrEmpty(SceneMemory.previousSceneName))
            {
                // 저장된 씬 이름이 있다면, 그 씬으로 돌아갑니다.
                SceneManager.LoadScene(SceneMemory.previousSceneName);
            }
            else
            {
                // 저장된 씬 이름이 없다면 (예: 타이틀에서 바로 온 경우), 기본값인 타이틀 씬으로 이동
                SceneManager.LoadScene("Title1");
            }
            return;
        }

        currentPageIndex++;
        ShowPage(currentPageIndex);
    }

    public void GoToPrevPage()
    {
        if (currentPageIndex <= 0)
        {
            return;
        }
        currentPageIndex--;
        ShowPage(currentPageIndex);
    }
}