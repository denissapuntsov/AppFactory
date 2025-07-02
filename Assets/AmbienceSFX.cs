using System;
using UnityEngine;
using UnityEngine.UI;

public class AmbienceSFX : MonoBehaviour
{
    private AudioSource _audioSource;
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        GameManager.OnEnter += Resume;
        FindAnyObjectByType<Pause>().GetComponent<Button>().onClick.AddListener(Toggle);
        GameManager.OnShiftComplete += Pause;
        GameManager.OnShiftIncomplete += Pause;
    }

    private void Resume()
    {
        if (_audioSource) _audioSource.Play();
    }

    private void Pause()
    {
        if (_audioSource) _audioSource.Pause();
    }

    private void Toggle()
    {
        if (_audioSource.isPlaying) _audioSource.Pause();
        else _audioSource.Play();
    }

    private void OnDisable()
    {
        GameManager.OnEnter += Resume;
        GameManager.OnShiftComplete -= Pause;
        GameManager.OnShiftIncomplete -= Pause;
    }
}
