using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 0.1f;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;
    
    #region Singleton Pattern
    
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    #endregion

    #region Events and Delegates

    public delegate void OnTapHandler();
    public static event OnTapHandler OnTap;

    public delegate void OnSwipeHandler(string direction);
    public static event OnSwipeHandler OnSwipe;

    #endregion
    
    public void Tap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _startPosition = GetTapPosition();
            return;
        }

        if (context.canceled)
        {
            _endPosition = GetTapPosition();
            CheckForSwipe();
            OnTap?.Invoke();
        }
    }

    private Vector3 GetTapPosition() => Pointer.current.position.ReadValue();

    private void CheckForSwipe()
    {
        Vector3 startWorldPosition = _startPosition;
        startWorldPosition.z = 1;
        startWorldPosition = Camera.main.ScreenToWorldPoint(startWorldPosition);

        Vector3 endWorldPosition = _endPosition;
        endWorldPosition.z = 1;
        endWorldPosition = Camera.main.ScreenToWorldPoint(endWorldPosition);
        
        float distanceSwiped = Vector3.Distance(startWorldPosition, endWorldPosition);
        
        if (distanceSwiped >= swipeThreshold)
        {
            string direction = "";
            
            float horizontalSwipe = _endPosition.x - _startPosition.x;
            float verticalSwipe = _endPosition.y - _startPosition.y;

            if (Mathf.Abs(horizontalSwipe) > Mathf.Abs(verticalSwipe))
            {
                direction = horizontalSwipe > 0 ? "right" : "left";
            }
            else
            {
                direction = verticalSwipe > 0 ? "up" : "down";
            }
            
            OnSwipe?.Invoke(direction);
        }
    }
}
