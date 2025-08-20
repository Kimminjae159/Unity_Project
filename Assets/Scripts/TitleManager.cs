using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject optionWin;

    //Start, Quit, 그외의 Title에 있는 버튼들을 할당
    public Button[] titleBtn;

    void Start()
    {
        optionWin.SetActive(false);
    }

    // Option창의 Confirm Button에 할당
    public void OnClickConfirm()
    {
        optionWin.SetActive(false);
        // GameManager.instance. <- 이곳에 변경사항을 저장
        titleBtnSetting(true);
    }

    // Option창의 Cancle Button에 할당
    public void OnClickCancle()
    {
        optionWin.SetActive(false);
        titleBtnSetting(true);
    }

    // Start Button에 할당
    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }

    // Option Button에 할당
    public void OnClickOption()
    {
        optionWin.SetActive(true);
        // 옵션 창의 버튼 외에는 활성화 되지 않도록 설정
        titleBtnSetting(false);
    }

    private void titleBtnSetting(bool setting)
    {
        foreach (Button btn in titleBtn)
        {
            btn.gameObject.SetActive(setting);
        }
    }

    // Quit Button에 할당
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
