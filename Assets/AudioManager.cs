using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle, SFXToggle;
    [SerializeField] private AudioMixer mixer;
    public AudioSource globalMusicAudioSource, stingerAudioSource;

    private void OnEnable()
    {
        FilterMusicOff();
        if (FindAnyObjectByType<GameManager>()) GameManager.OnStart += SetVolumeToZero;
        
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        SFXToggle.onValueChanged.AddListener(ToggleSFX);
        
        musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        SFXToggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        
        ToggleMusic(musicToggle.isOn);
        ToggleSFX(musicToggle.isOn);
    }

    private void ToggleMusic(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn",  isOn ? 1 : 0);
        PlayerPrefs.Save();
        mixer.SetFloat("VolumeMusic", PlayerPrefs.GetInt("MusicOn") == 1 ? 0f : -80f);
    }

    private void ToggleSFX(bool isOn)
    {
        PlayerPrefs.SetInt("SFXOn",  isOn ? 1 : 0);
        PlayerPrefs.Save();
        mixer.SetFloat("VolumeSFX", PlayerPrefs.GetInt("SFXOn") == 1 ? 0f : -80f);
    }

    public void FilterMusicOn()
    {
        mixer.SetFloat("MasterLPF", 700);
    }

    public void FilterMusicOff()
    {
        mixer.SetFloat("MasterLPF", 22000);
    }

    public void SetVolumeToZero()
    {
        globalMusicAudioSource.volume = 0;
    }

    private void OnDisable()
    {
        if (FindAnyObjectByType<GameManager>())  GameManager.OnStart -= SetVolumeToZero;
        
        musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        SFXToggle.onValueChanged.RemoveListener(ToggleSFX);
    }
}
