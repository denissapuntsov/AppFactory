using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Singleton Pattern
    
    public static CustomerUI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    #endregion
    
    public GameObject customerIconParent;
    [SerializeField] private CanvasGroup canvasGroup;
    public Image emotionIcon;
    private void Start()
    {
        customerIconParent.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Idle) return;
        GameManager.CurrentGameState = GameState.Drag;
        
        Debug.Log("Begin Drag " + gameObject.name);
        customerIconParent.SetActive(true);
        customerIconParent.transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        customerIconParent.transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;

        canvasGroup.alpha = 1f;
        customerIconParent.SetActive(false);

        GameManager.CurrentGameState = GameState.Idle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        customerIconParent.SetActive(true);
        customerIconParent.transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        customerIconParent.SetActive(false);
    }
}
