using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] AudioSource mainMusic, vinyl;
    [SerializeField] private Button freePlayButton;
    private void OnEnable()
    {
        freePlayButton.onClick.AddListener(StopMusic);
    }

    private void StopMusic()
    {
        mainMusic.DOFade(0f, 0.3f);
    }
}
