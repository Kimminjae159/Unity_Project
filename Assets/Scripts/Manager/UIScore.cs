// 파일 이름: UIScore.cs
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    void OnEnable() { if (GameManager.instance != null) GameManager.ScoreUpdateCall += UpdateScoreText; }
    void OnDisable() { if (GameManager.instance != null) GameManager.ScoreUpdateCall -= UpdateScoreText; }
    void Start() { UpdateScoreText(); }
    private void UpdateScoreText()
    {
        if (GameManager.instance != null)
        {
            scoreText.text = GameManager.instance.score.ToString("N0");
            int combo = GameManager.instance.comboCount;
            comboText.text = combo != 0 ? combo.ToString()+"x" : "0";
        }

    }
}