using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Start,
    Enter,
    Idle,
    Select,
    Drag,
    Place,
    Paused,
    Dialogue,
    ShiftComplete,
    ShiftIncomplete,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button[] quitButtons;
    
    private static GameManager _instance;

    #region Events

    public delegate void OnStartHandler();
    public static event OnStartHandler OnStart;
    
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
    
    public delegate void OnPausedHandler();
    public static event OnPausedHandler OnPaused;
    
    public delegate void OnDialogueHandler();
    public static event OnDialogueHandler OnDialogue;
    
    public delegate void OnShiftSuccessHandler();
    public static event OnShiftSuccessHandler OnShiftComplete;
    
    public delegate void OnShiftIncompleteHandler();
    public static event OnShiftIncompleteHandler OnShiftIncomplete;

    #endregion

    private static GameState _currentGameState;
    
    public static GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
            Debug.Log(_currentGameState);
            switch (_currentGameState)
            {
                case GameState.Start:
                    OnStart?.Invoke();
                    break;
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
                case GameState.Paused:
                    OnPaused?.Invoke();
                    break;
                case GameState.Dialogue:
                    OnDialogue?.Invoke();
                    break;
                case GameState.ShiftComplete:
                    OnShiftComplete?.Invoke();
                    break;
                case GameState.ShiftIncomplete:
                    OnShiftIncomplete?.Invoke();
                    break;
            }
        }
    }

    private void OnEnable()
    {
        if (!resetButton) return;
        resetButton.onClick.AddListener(() => CurrentGameState = GameState.Enter);
        foreach (var quitButton in quitButtons) quitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        });
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    private void Start()
    {
        CurrentGameState = GameState.Start;
    }

    private void OnDisable()
    {
        if (!resetButton) return;
        resetButton.onClick.RemoveListener(() => CurrentGameState = GameState.Enter);
    }
}
