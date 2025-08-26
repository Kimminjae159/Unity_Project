// 파일 이름: GameOver.cs (5개의 using 구문을 모두 포함한 최종 버전)

using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.
using UnityEngine.Events;        // Events 관련 기능 사용 시 필요합니다.
using UnityEngine.UI;            // UI 관련 기능(Button, Image 등) 사용 시 필요합니다.
using TMPro;                     // TextMeshPro를 사용하기 위해 필요합니다.

public class GameOver : MonoBehaviour
{
    // --- UI 요소 연결 ---
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverText;
    public GameObject buttonParent;

    private bool AllMemberOK = true;

    void Awake()
    {
        if (!gameOverUI)
        {
            Debug.LogError("GameOver 스크립트에 gameOverUI가 할당되지 않았습니다.");
            AllMemberOK = false;
        }
        if (!gameOverText)
        {
            Debug.LogError("GameOver 스크립트에 gameOverText가 할당되지 않았습니다.");
            AllMemberOK = false;
        }
        if (!buttonParent)
        {
            Debug.LogError("GameOver 스크립트에 buttonParent가 할당되지 않았습니다.");
            AllMemberOK = false;
        }
        if (!AllMemberOK) return;

        // 시작 시 비활성화
        gameOverText.gameObject.SetActive(false);
        buttonParent.SetActive(false);
        gameOverUI.SetActive(false);
    }

    /// <summary>
    /// 게임 오버 시 외부(기존 HP 또는 GameManager)에서 호출할 함수
    /// </summary>
    public void OnGameOver(DialogueAsset dialogue)
    {
        // ScoreManager가 존재하면, 최종 점수를 저장하라고 명령
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.FinalizeScoreAndSave();
        }

        // 기존 게임오버 로직 실행
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        DialogueManager.instance.StartDialogue(dialogue, AfterDialoguePrint);
    }

    private void AfterDialoguePrint()
    {
        buttonParent.SetActive(true);
    }

    // '재시작' 버튼에 연결할 함수
    public void RestartGame()
    {
        // 재시작 시 다음 플레이를 위해 ScoreManager 파괴
        if (ScoreManager.instance != null)
        {
            Destroy(ScoreManager.instance.gameObject);
        }
        // 기존 GameManager의 재시작 함수 호출
        GameManager.instance.RestartLevel();
    }

    // '타이틀로' 버튼에 연결할 함수
    public void QuitGame()
    {
        // 타이틀로 돌아갈 때도 ScoreManager 파괴
        if (ScoreManager.instance != null)
        {
            Destroy(ScoreManager.instance.gameObject);
        }
        // 기존 GameManager의 타이틀 이동 함수 호출
        GameManager.instance.GoToTitle();
    }
}