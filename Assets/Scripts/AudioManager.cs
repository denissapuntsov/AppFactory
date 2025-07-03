using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle, sfxToggle;
    [SerializeField] private AudioMixer mixer;
    public AudioSource globalMusicAudioSource, stingerAudioSource;

    private void OnEnable()
    {
        FilterMusicOff();
        if (FindAnyObjectByType<GameManager>()) GameManager.OnStart += SetVolumeToZero;
        
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        sfxToggle.onValueChanged.AddListener(ToggleSFX);
    }

    private void Start()
    {
        musicToggle.onValueChanged.Invoke(PersistentGameInfo.Instance.isMusicOn);
        sfxToggle.onValueChanged.Invoke(PersistentGameInfo.Instance.isSfxOn);
    }

    private void ToggleMusic(bool isOn)
    {
        PersistentGameInfo.Instance.isMusicOn = isOn;
        mixer.SetFloat("VolumeMusic", PersistentGameInfo.Instance.isMusicOn ? 0f : -80f);
    }

    private void ToggleSFX(bool isOn)
    {
        PersistentGameInfo.Instance.isSfxOn = isOn;
        mixer.SetFloat("VolumeSFX", PersistentGameInfo.Instance.isSfxOn ? 0f : -80f);
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

    private void OnDestroy()
    {
        if (FindAnyObjectByType<GameManager>()) GameManager.OnStart -= SetVolumeToZero;
        
        musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        sfxToggle.onValueChanged.RemoveListener(ToggleSFX);
    }
}
