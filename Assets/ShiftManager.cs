using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shiftInfoField;
    [SerializeField] private Canvas endShiftCanvas;
    
    private DialogueManager _dialogueManager;
    private ScoreManager _scoreManager;
    private CircleManager _circleManager;

    private int _currentShiftIndex = -1;
    
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
        _dialogueManager.SetRegularBlock();
        _currentShiftIndex++;
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
        GameManager.OnEnter -= RandomizeShiftLength;
        GameManager.OnPlace -= GetNextCustomer;
    }
}
