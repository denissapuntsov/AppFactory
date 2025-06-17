using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Enter,
    Idle,
    Select,
    Drag,
    Place,
    Exit,
    Dialogue
}

public class GameManager : MonoBehaviour
{
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
    
    public delegate void OnExitHandler();
    public static event OnExitHandler OnExit;
    
    public delegate void OnDialogueHandler();
    public static event OnDialogueHandler OnDialogue;

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
                case GameState.Exit:
                    OnExit?.Invoke();
                    break;
                case GameState.Dialogue:
                    OnDialogue?.Invoke();
                    break;
            }
        }
    }
    
    [SerializeField] private TextMeshProUGUI debugStateText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    private void Start()
    {
        CurrentGameState = GameState.Idle;
    }
}
