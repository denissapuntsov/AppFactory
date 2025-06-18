using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float _time;

    private float Time
    {
        get => _time;
        set
        {
            // set hours and minutes
            // $"{hours}:{minutes}{time_indicator};
        }
    }
    
    private void OnEnable()
    {
        GameManager.OnEnter += (() => Time = 9.0f);
    }

    private void AddHalfHour()
    {
        Time += Time % 2 == 0 ? 0.5f : 1f;
    }
}
