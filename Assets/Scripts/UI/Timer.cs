using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float remainingTime;

    public GameOver gameOver;

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
                timerText.color = Color.red;
            }
            TimeCountdown();
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        gameOver.EndingFunc();
    }
}
