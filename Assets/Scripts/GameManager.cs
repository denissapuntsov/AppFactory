using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public enum GameState
{
    Enter,
    Idle,
    Select,
    Drag,
    Place,
    Dialogue,
    Score
}

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stateText;

    [SerializeField] private Button resetButton;
    
    private static GameManager _instance;

    #region Events

    public delegate void OnEnterHandler();
    public static event OnEnterHandler OnEnter;

    public delegate void OnIdleHandler();
    public static event OnIdleHandler OnIdle;
    
    public delegate void OnSelectHandler();
    public static event OnSelectHandler OnSelect;
    
    public delegate void OnDragHandler();
    public static event OnDragHandler OnDrag;
    
    public delegate void OnPlaceHandler();
    public static event OnPlaceHandler OnPlace;
    
    public delegate void OnDialogueHandler();
    public static event OnDialogueHandler OnDialogue;
    
    public delegate void OnScoreHandler();
    public static event OnScoreHandler OnScore;

    #endregion
    
    private static GameState _currentGameState = GameState.Enter;
    
    public static GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
            switch (_currentGameState)
            {
                case GameState.Enter:
                    OnEnter?.Invoke();
                    break;
                case GameState.Idle:
                    OnIdle?.Invoke();
                    break;
                case GameState.Select:
                    OnSelect?.Invoke();
                    break;
                case GameState.Drag:
                    OnDrag?.Invoke();
                    break;
                case GameState.Place:
                    OnPlace?.Invoke();
                    break; 
                case GameState.Dialogue:
                    OnDialogue?.Invoke();
                    break;
                case GameState.Score:
                    OnScore?.Invoke();
                    break;
            }
        }
    }

    private void OnEnable()
    {
        if (!resetButton) return;
        resetButton.onClick.AddListener(() => CurrentGameState = GameState.Enter);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    private void Start()
    {
        CurrentGameState = GameState.Enter;
    }

    private void Update()
    {
        stateText.text = $"GameState: {_currentGameState.ToString()}";
    }

    public void SetState(GameState newState)
    {
        _currentGameState = newState;
    }

    private void OnDisable()
    {
        if (!resetButton) return;
        resetButton.onClick.RemoveListener(() => CurrentGameState = GameState.Enter);
    }
}
