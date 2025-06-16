using DG.Tweening;
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
    
    public GameObject customerParent;
    private Vector3 _defaultPosition;
    private Vector3 _defaultScale;
    
    private void Start()
    {
        customerParent.SetActive(true);
        _defaultPosition = customerParent.transform.position;
        _defaultScale = customerParent.transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Idle) return;
        GameManager.CurrentGameState = GameState.Drag;
        
        Debug.Log("Begin Drag " + gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        customerParent.transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;

        GameManager.CurrentGameState = GameState.Idle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Sequence focusSequence = DOTween.Sequence();

        focusSequence.Join(customerParent.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Sequence unfocusSequence = DOTween.Sequence();

        unfocusSequence
            .Join(customerParent.transform.DOScale(_defaultScale, 0.2f))
            .Join(customerParent.transform.DOMove(_defaultPosition, 0.2f));
    }
}
