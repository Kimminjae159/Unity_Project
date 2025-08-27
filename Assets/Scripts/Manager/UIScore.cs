// 파일 이름: UIScore.cs
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    void OnEnable() { if (GameManager.instance != null) GameManager.ScoreUpdateCall += UpdateScoreText; }
    void OnDisable() { if (GameManager.instance != null) GameManager.ScoreUpdateCall -= UpdateScoreText; }
    void Start() { UpdateScoreText(); }
    private void UpdateScoreText() { if (GameManager.instance != null) scoreText.text = "Score: " + GameManager.instance.score.ToString("N0"); }
}