using UnityEngine;
using TMPro;
using System;

public class CountdownTimer : MonoBehaviour
{
    public static float currentTime;
    public static string currentTimeText;

    public TMP_Text countdownTimeUI;

    void Awake()
    {
        currentTime = 0f;
        UpdateCountdownText();
    }

    void Update()
    {
        if (MoveSpeed.speed > 0)
        {
            currentTime += Time.deltaTime;
            UpdateCountdownText();
        }
    }

    void UpdateCountdownText()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        countdownTimeUI.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        currentTimeText = $"{time.Minutes:D2}:{time.Seconds:D2}";
    }
}
