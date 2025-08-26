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

    void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ShowPage(currentPageIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Next 튜토리얼 창" + currentPageIndex);
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
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "창 닫기";
        }
        else
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "다음";
        }
    }

    public void GoToNextPage()
    {
        // 마지막 페이지라면 해당 함수가 호출될 경우 UI를 비활성화
        if (currentPageIndex >= pages.Length - 1)
        {
            currentPageIndex = 0;
            gameObject.SetActive(false);
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