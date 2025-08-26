// 파일 이름: ScoreManager.cs (최종 업그레이드 버전)

using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가!
using System.Collections.Generic; // 리스트, HashSet 등을 사용하기 위해 추가!

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [Header("점수 데이터")]
    public int score = 0;
    private int comboCount = 0;

    [Header("새로운 점수 규칙")]
    private const int BASE_SCORE = 100;  // 첫 발판 기본 점수
    private const int COMBO_BONUS = 10;   // 연속으로 밟을 때마다 추가되는 점수
    private const int CLEAR_BONUS = 500;  // 레벨 클리어 보너스

    // 밟았던 발판을 기억하기 위한 저장소 (HashSet은 검색 속도가 매우 빠릅니다)
    private HashSet<int> steppedOnPlatforms = new HashSet<int>();

    public event System.Action OnScoreUpdated;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 씬이 로드될 때마다 OnSceneLoaded 함수를 실행하도록 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 씬이 새로 로드되면(새 레벨 시작) 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 레벨이 시작되었으니, 밟았던 발판 기록을 모두 초기화
        steppedOnPlatforms.Clear();
    }

    /// <summary>
    /// 올바른 발판을 밟았을 때 호출됩니다.
    /// </summary>
    public void AddScoreForCorrectStep(GameObject platform)
    {
        // 이 발판의 고유 ID를 가져옵니다.
        int platformID = platform.GetInstanceID();

        // 만약 이 발판을 "이미 밟았다면" 점수를 주지 않고 즉시 함수를 종료합니다.
        if (steppedOnPlatforms.Contains(platformID))
        {
            return;
        }

        // 처음 밟는 발판이라면, 기록에 추가합니다.
        steppedOnPlatforms.Add(platformID);

        // 새로운 콤보 점수 계산
        // 콤보 0 (첫번째) = 100 + (10 * 0) = 100점
        // 콤보 1 (두번째) = 100 + (10 * 1) = 110점
        int earnedScore = BASE_SCORE + (COMBO_BONUS * comboCount);
        score += earnedScore;

        // 다음 콤보를 위해 콤보 카운트를 1 증가시킵니다.
        comboCount++;

        OnScoreUpdated?.Invoke(); // 점수 UI에 변경사항을 알립니다.
    }

    /// <summary>
    /// 잘못된 발판을 밟았을 때 호출됩니다. (콤보만 초기화)
    /// </summary>
    public void ResetCombo()
    {
        // 콤보 카운트를 0으로 되돌려, 다음 정답 발판은 다시 100점부터 시작하게 합니다.
        comboCount = 0;
    }

    /// <summary>
    /// 레벨 클리어 시 호출됩니다.
    /// </summary>
    public void AddScoreForLevelClear()
    {
        score += CLEAR_BONUS;
        comboCount = 0; // 다음 레벨을 위해 콤보 초기화
        OnScoreUpdated?.Invoke();
    }

    // ... (기존의 FinalizeScoreAndSave, SaveToLeaderboard 함수는 그대로 유지) ...
    public void FinalizeScoreAndSave()
    {
        Debug.Log("게임 오버! 최종 점수 저장: " + score);
        SaveScoreToLeaderboard(score);
    }

    private void SaveScoreToLeaderboard(int finalScore)
    {
        string scores = PlayerPrefs.GetString("LeaderboardScores", "");
        PlayerPrefs.SetString("LeaderboardScores", scores + finalScore.ToString() + ",");
        PlayerPrefs.Save();
    }
}