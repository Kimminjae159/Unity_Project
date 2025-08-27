using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using System.Linq;
using System.Text;
using UnityEngine.Rendering;

public class ResultSceneManager : MonoBehaviour
{
    public TextMeshProUGUI ResultText;
    [Header("State 1: Name Input")]
    public GameObject nameInput;
    public TMP_InputField nickNameInput; // TMP_InputField로 변경
    public GameObject confirmButton1;

    [Header("State 2: Score Display")]
    public GameObject scoreDisplayPanel; // 점수 표시 UI들의 부모 오브젝트
    public TMP_Text comboScoreText;      // (TMPro1)
    public TMP_Text clearTime1Text;      // (TMPro2)
    public TMP_Text clearTime2Text;      // (TMPro3)
    public TMP_Text clearTime3Text;      // (TMPro4)
    public TMP_Text clearTime4Text;      // (TMPro5)
    public TMP_Text totalScoreText;      // 계산 결과
    public GameObject confirmButton2;

    [Header("State 3: Leaderboard")]
    public GameObject leaderboardPanel;  // 리더보드와 타이틀 버튼의 부모 오브젝트
    public TMP_Text leaderboardText;     // 리더보드를 표시할 TMP_Text
    public GameObject titleButton;

    public GameObject ResultUI;
    private const string LeaderboardKey = "Leaderboard";

    // 외부에서 Result 창을 띄우기 위해 호출하는 함수
    public void ApplyResultUI()
    {
        ResultUI.SetActive(true);
        // 시작 시 초기 UI 상태 설정 (State 1: 이름 입력)
        nameInput.SetActive(true);
        ResultText.text = "RESULT";

        scoreDisplayPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    void Start()
    {
        ResultUI.SetActive(false);
    }

    /// <summary>
    /// Confirm Button 1에 할당할 함수
    /// </summary>
    public void OnConfirmName()
    {
        // 1) 입력된 텍스트를 GameManager에 저장
        string username = nickNameInput.text;
        if (string.IsNullOrWhiteSpace(username))
        {
            // 비어있는 이름은 저장하지 않음 (필요 시 경고 UI 추가)
            GameManager.instance.Username = "NULL";
        }
        else
        {
            GameManager.instance.Username = username;
        }

        // 2) UI 상태 변경 (State 1 -> State 2)
        nameInput.SetActive(false);
        scoreDisplayPanel.SetActive(true);

        // 3) GameManager에서 점수/시간을 불러와 TMPro에 표시
        DisplayScores();
    }

    /// <summary>
    /// Confirm Button 2에 할당할 함수
    /// </summary>
    public void OnConfirmScoresAndShowLeaderboard()
    {
        // 1) 이름과 총 점수를 리더보드에 저장
        SaveResultToLeaderboard();

        // 2) UI 상태 변경 (State 2 -> State 3)
        ResultText.text = "Leader Board";
        scoreDisplayPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        // 3) 저장된 기록을 포함하여 리더보드 표시
        LoadAndDisplayLeaderboard();
    }

    // --- Private Helper Methods ---

    /// <summary>
    /// GameManager의 데이터를 불러와 점수판 UI에 표시하는 함수
    /// </summary>
    private void DisplayScores()
    {
        // comboScore 표시 (int 값이므로 null 체크 대신 0과 비교)
        int comboScore = GameManager.instance.comboScore;
        comboScoreText.text = comboScore > 0 ? comboScore.ToString() : "--";

        // ClearTime 표시 (float 값이므로 0f와 비교, 소수점 2자리까지 표시)
        clearTime1Text.text = GameManager.instance.ClearTime1 > 0 ? GameManager.instance.ClearTime1.ToString() : "--";
        clearTime2Text.text = GameManager.instance.ClearTime2 > 0 ? GameManager.instance.ClearTime2.ToString() : "--";
        clearTime3Text.text = GameManager.instance.ClearTime3 > 0 ? GameManager.instance.ClearTime3.ToString() : "--";
        clearTime4Text.text = GameManager.instance.ClearTime4 > 0 ? GameManager.instance.ClearTime4.ToString() : "--";

        int totalClearTime = GameManager.instance.ClearTime1 +
                           GameManager.instance.ClearTime2 +
                           GameManager.instance.ClearTime3 +
                           GameManager.instance.ClearTime4;

        // 시간 보너스 계산 (최소 0점)
        int timeBonus = totalClearTime * 10;
        if (timeBonus < 0)
        {
            timeBonus = 0; // 보너스 점수가 음수가 되지 않도록 방지
        }
        int totalScore = comboScore + timeBonus;
        GameManager.instance.TotalScore = totalScore;
        // TotalScore 표시
        totalScoreText.text = totalScore.ToString();//
    }

    /// <summary>
    /// 현재 게임 결과를 PlayerPrefs에 JSON 형식으로 저장하는 함수
    /// </summary>
    private void SaveResultToLeaderboard()
    {
        string json = PlayerPrefs.GetString(LeaderboardKey, "{}");
        LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);

        leaderboardData.results.Add(new PlayerResult
        {
            nickName = GameManager.instance.Username,
            score = GameManager.instance.TotalScore
        });

        string updatedJson = JsonUtility.ToJson(leaderboardData);
        PlayerPrefs.SetString(LeaderboardKey, updatedJson);
        PlayerPrefs.Save();
        Debug.Log("리더보드에 결과 저장 완료!");
    }

    /// <summary>
    /// 저장된 리더보드 데이터를 불러와 상위 3개를 UI에 표시하는 함수
    /// </summary>
    private void LoadAndDisplayLeaderboard()
    {
        string json = PlayerPrefs.GetString(LeaderboardKey, "{}");
        LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);

        if (leaderboardData.results == null || leaderboardData.results.Count == 0)
        {
            leaderboardText.text = "저장된 기록이 없습니다.";
            return;
        }

        var top3Results = leaderboardData.results.OrderByDescending(r => r.score).Take(3);

        StringBuilder builder = new StringBuilder();
        int rank = 1;
        foreach (var result in top3Results)
        {
            builder.AppendLine($"{rank}.  {result.nickName}\t:  {result.score} 점");
            rank++;
        }

        leaderboardText.text = builder.ToString();
    }
    public void GoTitle()
    {
        GameManager.instance.GoToTitle();
    }
}