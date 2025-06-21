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
    }

    private void Reset()
    {
        StopCoroutine(CountTime());
        Time = 9.0f;
    }

    private IEnumerator CountTime()
    {
        yield return new WaitForSecondsRealtime(7.5f);
        Time += 0.5f;
        if (Time >= 17.0f)
        {
            GameManager.CurrentGameState = GameState.Score;
            StopCoroutine(CountTime());
            yield break;
        }
        StartCoroutine(CountTime());
    }
}
