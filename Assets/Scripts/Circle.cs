using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{

    #region Fields and Properties

    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private GameObject startRef, endRef;
    
    private ScoreManager _scoreManager;
    private bool _isOverlappingCustomerIcon;
    private RectTransform _circleReferenceTransform;
    private CustomerUI _customerUI;
    private GameObject _customerPortrait;

    private Vector3 _defaultScale;

    public int Index { get; private set; }
    public CircleEnvironment environment;
    public CircleTemperature temperature;

    #endregion

    private void Start()
    {
        _defaultScale = transform.localScale;
        
        Index = transform.GetSiblingIndex();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _circleReferenceTransform = GameObject.FindWithTag("CircleReferenceTransform").GetComponent<RectTransform>();
        _customerUI = FindAnyObjectByType<CustomerUI>();
        _customerPortrait = _customerUI.customerParent;
        
        temperature = Index <= 3 ? CircleTemperature.Cold : CircleTemperature.Hot; 
        
        indexText.text = $"Index: {Index},\nEnvironment: {environment}\nTemperature: {temperature}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        transform.DOScale(_defaultScale, 0.2f);
        eventData.pointerDrag = null;
        _scoreManager.AddPoints(Customer.Instance, circle: this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;

        _customerPortrait.transform.SetParent(transform.GetComponentInChildren<Mask>().transform, true);
        _customerPortrait.transform.SetAsLastSibling();

        CircleManager.CurrentCircle = this;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;

        _customerUI.SetCustomerLayerToDrag();

        CircleManager.CurrentCircle = null;
        return;
    }
}
