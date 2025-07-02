using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shiftInfoField;
    [SerializeField] private Canvas endShiftCanvas;
    
    public bool shiftHasDialogue;
    
    private DialogueManager _dialogueManager;
    private ScoreManager _scoreManager;
    private CircleManager _circleManager;
    private Timer _timer;
    [SerializeField] private AudioManager _audioManager;

    private int _currentShiftIndex = -1;
    
    public int CurrentShiftIndex => _currentShiftIndex;

    private int _currentCustomerIndex;
    
    public static ShiftManager Instance;
    public int CurrentCustomerIndex
    {
        get => _currentCustomerIndex;
        set
        {
            _currentCustomerIndex = value;
            shiftInfoField.text = $"{_currentCustomerIndex + 1}/{_shiftLength}";
        }
    }

    private int _shiftLength;
    public int ShiftLength
    {
        get => _shiftLength;
        set
        {
            _shiftLength = value;
            shiftInfoField.text = $"{_currentCustomerIndex + 1}/{_shiftLength}";
        }
    }
    
    private void OnEnable()
    {
        Instance = this;
        _dialogueManager = FindAnyObjectByType<DialogueManager>();
        _circleManager = FindAnyObjectByType<CircleManager>();

        GameManager.OnEnter += InitializeShift;
        GameManager.OnPlace += GetNextCustomer;
    }

    private void InitializeShift()
    {
        _audioManager.globalMusicAudioSource.mute = false;
        _audioManager.globalMusicAudioSource.volume = 0f;
        _audioManager.globalMusicAudioSource.DOFade(1f, 1f);
        _currentShiftIndex++;

        // gradually reduce time on subsequent shifts
        _timer.InterpolatedPeriod -= _currentShiftIndex / 10f;
        
        _dialogueManager.ConfigurePanel(_currentShiftIndex);
        
        CurrentCustomerIndex = 0;
        RandomizeShiftLength();

        if (_currentShiftIndex >= 2)
        {
            _circleManager.BlockRandomCircle(1);
        }
    }

    private void GetNextCustomer()
    {
        if (_currentCustomerIndex + 1 == _shiftLength)
        {
            Debug.Log("Lest customer served; transitioning to scoring"); 
            GameManager.CurrentGameState = GameState.ShiftComplete;
            return;
        }
        
        CurrentCustomerIndex++;
    }

    private void RandomizeShiftLength()
    {
        ShiftLength = Random.Range(5, 9);
    }

    private void OnDisable()
    {
        GameManager.OnEnter -= InitializeShift;
        GameManager.OnPlace -= GetNextCustomer;
    }
}
