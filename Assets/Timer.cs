using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject timerUI;
    private TextMeshProUGUI _timerText;

    private static float _gameTime;
    private bool _isRunning = false;
    private float _innerTime;
    private float _interpolatedPeriod = 2f;

    public float InterpolatedPeriod
    {
        get => _interpolatedPeriod;
        set
        {
            if (value < 1f) value = 1f;
            _interpolatedPeriod = value;
        }
    }

    public float GameTime
    {
        get => _gameTime;
        private set
        {
            _gameTime = value;
            
            string halfOfDay = "AM";
            if (Mathf.Floor(value) >= 12) halfOfDay = "PM";
            
            string minutes = value * 10 % 10 == 0 ? "00" : "30";
            
            string hours;
            if (Mathf.Floor(value) > 12) value -= 12;
            hours = $"{Mathf.Floor(value)}";
            
            _timerText.text = $"{hours}:{minutes} {halfOfDay}";
        }
    }
    
    private void OnEnable()
    {
        timerUI.SetActive(true);
        _timerText = timerUI.GetComponentInChildren<TextMeshProUGUI>();
        
        GameTime = 9.0f;
        GameManager.OnEnter += () =>
        {
            GameTime = 9.0f;
        };
        DialoguePanel.OnEndDialogue += () =>
        {
            Reset();
            _isRunning = true;
        };
        GameManager.OnShiftComplete += () =>
        {
            Reset();
            _isRunning = false;
        };
    }

    private void Update()
    {
        if (_isRunning)
        {
            _innerTime += Time.deltaTime;

            if (_innerTime >= _interpolatedPeriod)
            {
                _innerTime = 0f;
                GameTime += 0.5f;
                if (GameTime >= 17.0)
                {
                    _isRunning = false;
                    GameManager.CurrentGameState = GameState.ShiftIncomplete;
                }
            }
        }
    }

    private void Reset()
    {
        _innerTime = 0f;
        GameTime = 9.0f;
    }

    /*private IEnumerator CountTime()
    {
        while (true)
        {
            if (GameTime >= 17.0f)
            {
                GameManager.CurrentGameState = GameState.ShiftIncomplete;
                StopCoroutine(CountTime());
                yield break;
            }
            yield return new WaitForSeconds(2f);
            GameTime += 0.5f;
        }
    }*/

    private void OnDisable()
    {
        timerUI.SetActive(false);
        GameManager.OnShiftComplete -= Reset;
    }
}
