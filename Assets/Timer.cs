using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private static float _time;
    private bool _isRunning;

    public float Time
    {
        get => _time;
        private set
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
        GameManager.OnEnter += () => Time = 9.0f;
        DialoguePanel.OnEndDialogue += () => StartCoroutine(CountTime());
        GameManager.OnShiftComplete += StopAllCoroutines;
        GameManager.OnShiftIncomplete += StopAllCoroutines;
    }

    private void Reset()
    {
        StopCoroutine(CountTime());
        Time = 9.0f;
    }

    private IEnumerator CountTime()
    {
        if (Time > 17.0f)
        {
            GameManager.CurrentGameState = GameState.ShiftIncomplete;
            StopCoroutine(CountTime());
            yield break;
        }
        yield return new WaitForSecondsRealtime(2f);
        Time += 0.5f;
        StartCoroutine(CountTime());
    }

    private void OnDisable()
    {
        GameManager.OnShiftComplete -= StopAllCoroutines;
        GameManager.OnShiftIncomplete -= StopAllCoroutines;
    }
}
