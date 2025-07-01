using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private static float _gameTime;
    private bool _isRunning = false;
    private float _innerTime;
    private float _interpolatedPeriod = 2f;

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
            
            timerText.text = $"{hours}:{minutes} {halfOfDay}";
        }
    }
    
    private void OnEnable()
    {
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
        GameManager.OnShiftComplete -= Reset;
    }
}
