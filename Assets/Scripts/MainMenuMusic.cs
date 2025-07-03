using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource mainMusic, vinyl;
    [SerializeField] private Button freePlayButton, tutorialButton;
    private void OnEnable()
    {
        freePlayButton.onClick.AddListener(StopMusic);
        tutorialButton.onClick.AddListener(StopMusic);
    }

    private void StopMusic()
    {
        mainMusic.DOFade(0f, 0.3f);
    }
}
