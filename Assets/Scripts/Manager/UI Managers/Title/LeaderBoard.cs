using UnityEngine;
using TMPro;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour
{
    public TMP_Text name1;
    public TMP_Text name2;
    public TMP_Text name3;
    public TMP_Text score1;
    public TMP_Text score2;
    public TMP_Text score3;
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
        // UI 컴포넌트들을 배열로 묶어 코드를 간결하게 만듭니다.
        TMP_Text[] names = { name1, name2, name3 };
        TMP_Text[] scores = { score1, score2, score3 };

        string json = PlayerPrefs.GetString(LeaderboardKey, "{}");
        LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);

        // 점수가 높은 순으로 정렬된 리스트를 가져옵니다.
        List<PlayerResult> topResults = leaderboardData.results.OrderByDescending(r => r.score).ToList();

        // 1등부터 3등까지 UI를 순차적으로 채웁니다.
        for (int i = 0; i < 3; i++)
        {
            // 해당 순위에 데이터가 존재하는지 확인합니다.
            if (i < topResults.Count)
            {
                // 데이터가 있으면 이름과 점수를 할당합니다.
                PlayerResult result = topResults[i];
                names[i].text = result.nickName;
                scores[i].text = result.score.ToString();
            }
            else
            {
                // 데이터가 없으면 "NULL"을 할당합니다.
                names[i].text = "---";
                scores[i].text = "---";
            }
        }
    }
    public void GoTitle()
    {
        gameObject.SetActive(false);
    }
}
