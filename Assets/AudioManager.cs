using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider, SFXVolumeSlider;

    private void OnEnable()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }
}
