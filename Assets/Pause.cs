using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Sprite pause, resume;
    [SerializeField] private List<GameObject> pausePanelElements;
    private AudioSource _audioSource;
    private AudioManager _audioManager;
    
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioManager = FindAnyObjectByType<AudioManager>();
        GetComponent<Button>().onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        if (GameManager.CurrentGameState is not GameState.Paused and not GameState.Idle) return;
        
        if (GameManager.CurrentGameState == GameState.Paused)
        {
            _audioManager.FilterMusicOff();
            _audioSource.Play();
            GetComponent<Image>().sprite = pause;
            Time.timeScale = 1f;
            GameManager.CurrentGameState = GameState.Idle;
        }
        else if (GameManager.CurrentGameState == GameState.Idle)
        {
            _audioManager.FilterMusicOn();
            _audioSource.Play();
            GetComponent<Image>().sprite = resume;
            Time.timeScale = 0f;
            GameManager.CurrentGameState = GameState.Paused;
        }

        foreach (GameObject pausePanel in pausePanelElements) pausePanel.SetActive(!pausePanel.activeSelf);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(TogglePause);
    }
}
