using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Sprite pause, resume;
    [SerializeField] private GameObject pausePanel;
    
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        if (GameManager.CurrentGameState is not GameState.Paused and not GameState.Idle) return;
        
        if (GameManager.CurrentGameState == GameState.Paused)
        {
            GetComponent<Image>().sprite = pause;
            Time.timeScale = 1f;
            GameManager.CurrentGameState = GameState.Idle;
        }
        else if (GameManager.CurrentGameState == GameState.Idle)
        {
            GetComponent<Image>().sprite = resume;
            Time.timeScale = 0f;
            GameManager.CurrentGameState = GameState.Paused;
        }

        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(TogglePause);
    }
}
