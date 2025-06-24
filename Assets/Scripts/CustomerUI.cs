using System;
using DG.Tweening;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI customerFlavorText;

    [SerializeField] private Transform officeParent, dragParent, hiddenTransform;
    public GameObject customerParent;
    private Vector3 _defaultPosition;
    private Vector3 _defaultScale;

    [SerializeField] private GameObject customerBackground, officeBackground;

    private void Start()
    {
        customerBackground.SetActive(true);
        GetComponent<CanvasGroup>().alpha = 0;
        officeBackground.SetActive(false);
        _defaultPosition = customerParent.transform.position;
        _defaultScale = customerParent.transform.localScale;
        HideCustomer();

        ShrinkBackground();
    }

    private void HideCustomer()
    {
        customerParent.transform.SetParent(officeParent);
        customerParent.transform.SetSiblingIndex(1);
        customerParent.transform.position = hiddenTransform.position;
    }

    private void ShrinkBackground()
    {
        Sequence shrinkBackgroundSequence = DOTween.Sequence();
        shrinkBackgroundSequence.SetEase(Ease.Linear)
            .Join(customerBackground.transform.DOScale(officeBackground.transform.localScale, 1f))
            .Join(customerBackground.transform.DOMove(officeBackground.transform.position, 1f))
            .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                customerBackground.SetActive(false);
                officeBackground.SetActive(true);
                GameManager.CurrentGameState = GameState.Enter;
                GetComponent<CanvasGroup>().alpha = 1;
                Appear();
            });
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
        if (GameManager.CurrentGameState != GameState.Idle) return;
        
        customerParent.transform.SetParent(dragParent);
        Sequence focusSequence = DOTween.Sequence();
        focusSequence.Join(customerParent.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState is not GameState.Drag and not GameState.Idle) return;

        Appear();
    }

    private void Appear()
    {
        HideCustomer();
        
        Sequence unfocusSequence = DOTween.Sequence();
        unfocusSequence
            .Join(customerParent.transform.DOScale(_defaultScale, 0.4f))
            .Join(customerParent.transform.DOMove(_defaultPosition, 0.4f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Idle);
    }

    private void Disappear()
    {
        // go into the circle mask object
        // go down
        // on complete set game state to idle
    }
}
