// 파일 이름: UIScore.cs
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    void OnEnable() { if (ScoreManager.instance != null) ScoreManager.instance.OnScoreUpdated += UpdateScoreText; }
    void OnDisable() { if (ScoreManager.instance != null) ScoreManager.instance.OnScoreUpdated -= UpdateScoreText; }
    void Start() { UpdateScoreText(); }
    private void UpdateScoreText() { if (ScoreManager.instance != null) scoreText.text = "Score: " + ScoreManager.instance.score.ToString("N0"); }
}