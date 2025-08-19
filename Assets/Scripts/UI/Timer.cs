using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Timer : MonoBehaviour
{
    [Header("UI_Player의 timerText를 할당")]
    public TextMeshProUGUI timerText;

    private float remainingTime;

    public static Timer instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }

    void Start()
    {
        remainingTime = GameManager.instance.timeLimit;
    }

    void TimeCountdown()
    {
        int min = Mathf.FloorToInt(remainingTime / 60);
        int sec = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = 0;
                StartCoroutine(GameOver());
            }
            TimeCountdown();
            if(remainingTime < 31) timerText.color = Color.red;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        StageManager.instance.HandlePlayerGameOver("timeOut");
    }
}
