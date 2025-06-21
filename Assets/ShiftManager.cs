using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shiftInfoField;
    [SerializeField] private Canvas endShiftCanvas;

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

        GameManager.OnEnter += InitializeShift;
        GameManager.OnPlace += GetNextCustomer;
    }

    private void InitializeShift()
    {
        _currentShiftIndex++;
        {
            
        }
        CurrentCustomerIndex = 0;
        RandomizeShiftLength();
    }

    private void GetNextCustomer()
    {
        if (_currentCustomerIndex + 1 == _shiftLength)
        {
            Debug.Log("Lest customer served; transitioning to scoring");
            GameManager.CurrentGameState = GameState.Score;
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
