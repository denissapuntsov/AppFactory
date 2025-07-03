using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shiftInfoField;
    [SerializeField] private Canvas endShiftCanvas;
    
    public bool shiftHasDialogue;
    
    private DialogueManager _dialogueManager;
    private ScoreManager _scoreManager;
    private CircleManager _circleManager;
    private Customer _customer;
    private Timer _timer;
    private Image _questionButtonImage;
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
        _timer = FindAnyObjectByType<Timer>();
        _questionButtonImage = FindAnyObjectByType<InfoTrigger>().GetComponent<Image>();
        _customer =  FindAnyObjectByType<Customer>();

        GameManager.OnEnter += InitializeShift;
        GameManager.OnPlace += GetNextCustomer;
    }

    private void InitializeShift()
    {
        _audioManager.globalMusicAudioSource.mute = false;
        _audioManager.globalMusicAudioSource.volume = 0f;
        _audioManager.globalMusicAudioSource.DOFade(1f, 1f);
        _currentShiftIndex++;

        // first shift: no timer, no question button, only neutral and temp/anti-temp demons
        // second shift: no timer, question button, all kinds of demons
        // third shift: add timer
        // fourth shift: start blocking circles
        
        List<CustomerType> customerTypePool;
        
        switch (_currentShiftIndex)
        {
            case 0:
                // disable timer and info button, use only temperature-relevant customer types
                _timer.gameObject.SetActive(false);
                _questionButtonImage.enabled = false;
                customerTypePool = new List<CustomerType>()
                {
                    CustomerType.Neutral,
                    CustomerType.Temp,
                    CustomerType.AntiTemp
                };
                break;
            case 1:
                // enable info button, use all non-complex customer types
                _questionButtonImage.enabled = true;
                customerTypePool = new List<CustomerType>()
                {
                    CustomerType.Neutral,
                    CustomerType.Temp,
                    CustomerType.AntiTemp,
                    CustomerType.Env,
                    CustomerType.AntiEnv
                };
                break;
            case 2: 
                //enable timer, use all customer types
                _timer.gameObject.SetActive(true);
                customerTypePool = new List<CustomerType>()
                {
                    CustomerType.Neutral,
                    CustomerType.Temp,
                    CustomerType.AntiTemp,
                    CustomerType.Env,
                    CustomerType.AntiEnv,
                    CustomerType.NegativeComplex,
                    CustomerType.PositiveComplex
                };
                break;
            default:
                _timer.gameObject.SetActive(true);
                _questionButtonImage.enabled = true;
                _circleManager.BlockRandomCircle(1);
                customerTypePool = new List<CustomerType>()
                {
                    CustomerType.Neutral,
                    CustomerType.Temp,
                    CustomerType.AntiTemp,
                    CustomerType.Env,
                    CustomerType.AntiEnv,
                    CustomerType.NegativeComplex,
                    CustomerType.PositiveComplex
                };
                break;
        }
        _customer.Randomise(customerTypePool);

        // gradually reduce time on subsequent shifts
        _timer.InterpolatedPeriod -= _currentShiftIndex / 10f;
        
        _dialogueManager.ConfigurePanel(_currentShiftIndex);
        FindAnyObjectByType<CustomerUI>().Appear();
        
        CurrentCustomerIndex = 0;
        RandomizeShiftLength();
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
