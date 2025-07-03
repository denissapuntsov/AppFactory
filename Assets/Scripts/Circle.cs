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
    [SerializeField] private GameObject planks;
    [SerializeField] private Image environmentImage;
    
    private ScoreManager _scoreManager;
    private CircleManager _circleManager;
    private bool _isOverlappingCustomerIcon;
    private CustomerUI _customerUI;
    private GameObject _customerPortrait;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip dropSound, blockedSound;


    private Vector3 _defaultScale;

    private bool _isBlocked;

    public bool IsBlocked
    {
        get => _isBlocked;
        set
        {
            _isBlocked = value;
            planks.SetActive(_isBlocked);
        }
    }
    
    public int Index { get; private set; }
    
    public CircleEnvironment environment;
    public CircleTemperature temperature;

    #endregion

    private void OnEnable()
    {
        Index = transform.GetSiblingIndex();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _defaultScale = transform.localScale;
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _customerUI = FindAnyObjectByType<CustomerUI>();
        _customerPortrait = _customerUI.customerParent;
        _circleManager = FindAnyObjectByType<CircleManager>();
        
        temperature = Index <= 4 ? CircleTemperature.Cold : CircleTemperature.Hot;
        
        environmentImage.sprite = _circleManager.GetCircleSprite(temperature, environment);
        
        indexText.text = $"Index: {Index},\nEnvironment: {environment}\nTemperature: {temperature}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        if (IsBlocked) return;
        
        _audioSource.PlayOneShot(dropSound);
        transform.DOScale(_defaultScale, 0.2f);
        eventData.pointerDrag = null;
        _scoreManager.AddPoints(Customer.Instance, circle: this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        if (IsBlocked)
        {
            _audioSource.PlayOneShot(blockedSound);
            return;
        }

        _customerPortrait.transform.SetParent(transform.GetComponentInChildren<Mask>().transform, true);
        _customerPortrait.transform.SetAsLastSibling();

        CircleManager.CurrentCircle = this;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        if (IsBlocked) return;

        _customerUI.SetCustomerLayerToDrag();

        CircleManager.CurrentCircle = null;
    }
}
