using System;
using UnityEngine;


public class PersistentGameInfo : MonoBehaviour
{
    public static PersistentGameInfo Instance { get; private set; }

    public bool isTutorial = false;
    public bool isSfxOn, isMusicOn;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

}
