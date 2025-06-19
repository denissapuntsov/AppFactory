using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float _time;

    private float Time
    {
        get => _time;
        set
        {
            _time = value;
            
            string halfOfDay = "AM";
            if (Mathf.Floor(value) >= 12) halfOfDay = "PM";
            
            string minutes = value * 10 % 10 == 0 ? "00" : "30";
            
            string hours;
            if (Mathf.Floor(value) > 12) value -= 12;
            hours = $"{Mathf.Floor(value)}";
            
            timerText.text = $"{hours}:{minutes} {halfOfDay}";
        }
    }
    
    private void OnEnable()
    {
        GameManager.OnEnter += (() =>
        {
            Time = 9.0f;
            StartCoroutine(CountTime());
        });
    }

    private void AddHalfHour()
    {
        Time += 0.5f;
    }

    private IEnumerator CountTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        AddHalfHour();
        StartCoroutine(CountTime());
    }
}
