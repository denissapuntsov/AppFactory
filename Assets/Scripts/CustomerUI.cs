using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CustomerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    public SpriteRenderer emotionIcon;
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
        customerIconParent.transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        customerIconParent.transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        Debug.Log("End Drag " + gameObject.name);
        customerIconParent.SetActive(false);

        GameManager.CurrentGameState = GameState.Idle;
    }
}
