using UnityEngine;
using TMPro;
using System.Linq;
using System.Text;

public class LeaderBoard : MonoBehaviour
{
    public TMP_Text leaderboardText;     // 리더보드를 표시할 TMP_Text
    public GameObject titleButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void OnEnable()
    {
        LoadAndDisplayLeaderboard();
    }

    private const string LeaderboardKey = "Leaderboard";

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
        gameObject.SetActive(false);
    }
}
