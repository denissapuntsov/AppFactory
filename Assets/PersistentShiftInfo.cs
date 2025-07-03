using System;
using UnityEngine;


public class PersistentShiftInfo : MonoBehaviour
{
    public static PersistentShiftInfo Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public bool isTutorial = false;
}
