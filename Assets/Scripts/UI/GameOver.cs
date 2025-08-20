using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    // 게임오버 UI를 인스펙터에서 연결
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverText;
    public GameObject buttonParent;

    private bool AllMemberOK = true;
    void Awake()
    {
        if (!gameOverUI)
        {
            Debug.LogError("PlayerUIManager 스크립트에 Player UI가 할당되지 않음.");
            AllMemberOK = false;
        }
        if (!gameOverText)
        {
            Debug.LogError("PlayerUIManager 스크립트에 Player UI가 할당되지 않음.");
            AllMemberOK = false;
        }
        if (!buttonParent)
        {
            Debug.LogError("PlayerUIManager 스크립트에 Player UI가 할당되지 않음.");
            AllMemberOK = false;
        }
        if (!AllMemberOK) return;
        gameOverText.gameObject.SetActive(false);
        buttonParent.SetActive(false);
        gameOverUI.SetActive(false);
    }
    
    /// <summary>
    /// 게임 오버시 외부에서 호출
    /// 1. 게임 오버 UI 활성화
    /// 2. GameOver Text 활성화
    /// 3. Dialogue 출력
    /// 4. Button 활성화
    /// </summary>
    public void OnGameOver(DialogueAsset dialogue)
    {
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);

        gameOverText.gameObject.SetActive(true);

        DialogueManager.instance.StartDialogue(dialogue, AfterDialoguePrint);
    }
    private void AfterDialoguePrint()
    {
        buttonParent.SetActive(true);
    }

    // '재시작' 버튼에 연결할 함수입니다.
    public void RestartGame()
    {
        // 현재 씬을 처음부터 다시 로드합니다.
        Debug.Log("restart");
        GameManager.instance.RestartLevel();
    }

    // '종료' 버튼에 연결할 함수입니다.
    public void QuitGame()
    {
        Debug.Log("quit");
        GameManager.instance.GoToTitle();
    }
}
