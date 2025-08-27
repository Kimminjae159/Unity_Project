using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 게임의 전반적인 상태와 데이터를 관리하며, 씬 전환 시에도 파괴되지 않습니다.
/// 싱글톤 패턴을 사용하여 어디서든 하나의 인스턴스에만 접근할 수 있도록 합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- 싱글톤 인스턴스 ---
    public static GameManager instance;

    // --- 게임 데이터 (씬 매니저가 참조) ---
    [Header("Game Core Data")]
    public int sympathyValue = 0;   // 공감 수치 
    public int healthPoint = 5;     // 최대 체력
    public int restartCount = 0;    // 재시작 횟수
    public float skyboxExposure = 1.3f; // 스카이박스 노출값
    public float timeLimit = 120f;  // 시간 제한 (초 단위)
    public bool isRestart = false;  // 현재 씬이 재시작된 것인지 여부

    public string Username = null;
    public int comboScore = 0;
    public int ClearTime1 = 0;
    public int ClearTime2 = 0;
    public int ClearTime3 = 0;
    public int ClearTime4 = 0;
    public int TotalScore = 0;

    [Header("점수 데이터")]
    public int score = 0;    // 점수 (필요한지 여부를 따져봐야 할듯함)
    private int comboCount = 0;

    [Header("새로운 점수 규칙")]
    private const int BASE_SCORE = 100;  // 첫 발판 기본 점수
    private const int COMBO_BONUS = 10;   // 연속으로 밟을 때마다 추가되는 점수
    private const int CLEAR_BONUS = 500;  // 레벨 클리어 보너스


    public static Action ScoreUpdateCall;
    // <summary>
    // 싱글톤 패턴 구현
    // </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// 호출 즉시 체력을 1 감소시킴과 동시에 체력 현황 반환
    /// 체력이 0 이하(=종료)로 떨어지면 true, 0 초과(=생존)이면 false
    /// </summary>
    public bool DecreaseHealth()
    {
        healthPoint--;
        return healthPoint <= 0;
    }

    /// <summary>
    /// 새로운 씬을 로드하기 전에 호출하여 데이터를 준비하는 용도
    /// 스텟 초기화가 주 목적
    /// </summary>
    /// <param name="isThisARestart">재시작하는 경우 true, 다음 레벨로 가는 경우 false</param>
    public void PrepareForNewScene(bool isThisARestart = false)
    {
        isRestart = isThisARestart; // isRestart : 현재씬을 재시작한 경우 true, 다음씬으로 넘어갈 경우 false가 됨

        // 공통 사항 : 재시작시 HP 복구
        healthPoint = 5;

        if (isThisARestart) // 재시작일 경우
        {
            restartCount++;
        }
        else  // 다음 씬으로 넘어간 경우
        {
            score += CLEAR_BONUS;
            comboCount = 0; // 다음 레벨을 위해 콤보 초기화
            ScoreUpdateCall?.Invoke();
        }
    }

    /// <summary>
    /// UI_GameOver의 'Restart' 버튼에 대응
    /// 게임을 재시작 : "현재 씬"을 다시 로드합니다.
    /// </summary>
    public void RestartLevel()
    {
        Debug.Log("GameManager의 restart 호출");
        PrepareForNewScene(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// UI_GameOver의 'Title로 이동' 버튼 대응
    /// 타이틀로 이동합니다.
    /// </summary>
    public void GoToTitle()
    {
        Debug.Log("Go to Title...");
        isRestart = false;
        comboCount = 0;
        score = 0;
        restartCount = 0;
        Username = null;
        comboScore = 0;
        ClearTime1 = 0;
        ClearTime2 = 0;
        ClearTime3 = 0;
        ClearTime4 = 0;
        TotalScore = 0;
        SceneManager.LoadScene(0);
    }



    
    // Correct 발판 -> PlayerReset에서 , Wrong 발판 -> SimpleMove에서
    public void PlayerStepPlatform(bool isCorrect)
    {
        if (isCorrect)
        {
            // 새로운 콤보 점수 계산
            // 콤보 0 (첫번째) = 100 + (10 * 0) = 100점
            // 콤보 1 (두번째) = 100 + (10 * 1) = 110점
            int earnedScore = BASE_SCORE + (COMBO_BONUS * comboCount);
            score += earnedScore;

            // 다음 콤보를 위해 콤보 카운트를 1 증가시킵니다.
            comboCount++;

            ScoreUpdateCall?.Invoke(); // 점수 UI에 변경사항을 알립니다.
        }
        else
        {
            // 콤보 카운트를 0으로 되돌려, 다음 정답 발판은 다시 100점부터 시작하게 합니다.
            comboCount = 0;
        }
    }
    
}
