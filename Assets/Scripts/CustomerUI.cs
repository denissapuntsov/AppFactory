using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI customerFlavorText;

    [SerializeField] private Transform officeParent, dragParent, question, counter;
    public GameObject customerParent;

    [Header("Transform References")] 
    [SerializeField] private Transform panelStart;
    [SerializeField] private Transform panelEnd;
    [SerializeField] private Transform customerStart;
    [SerializeField] private Transform customerEnd;
    [SerializeField] private Transform questionStart;
    [SerializeField] private Transform questionEnd;
    [SerializeField] private Transform counterStart;
    [SerializeField] private Transform counterEnd;
    // wow, this format sucks
    
    private Vector3 _defaultCustomerScale;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip tapSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        transform.parent.position = panelStart.position;
        _defaultCustomerScale = customerParent.transform.localScale;
        question.position  = questionStart.position;
        counter.position = counterStart.position;
        
        HideCustomer();

        transform.parent.DOMove(panelEnd.position, 1f)
            .OnComplete(() =>
            {
                GameManager.CurrentGameState = GameState.Enter;
                Appear();
                question.transform.DOMove(questionEnd.position, 1f);
                counter.transform.DOMove(counterEnd.position, 1f);
            });
    }

    private void HideCustomer()
    {
        customerParent.transform.SetParent(officeParent);
        customerParent.transform.SetSiblingIndex(1);
        customerParent.transform.position = customerStart.position;
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
        
        _audioSource.PlayOneShot(tapSound);
        SetCustomerLayerToDrag();
        customerParent.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f);
    }

    public void SetCustomerLayerToDrag() => customerParent.transform.SetParent(dragParent, true);
    

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
            .Join(customerParent.transform.DOScale(_defaultCustomerScale, 0.4f))
            .Join(customerParent.transform.DOMove(customerEnd.position, 0.4f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Idle);
    }
}
