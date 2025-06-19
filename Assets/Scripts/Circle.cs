using System;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Circle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{

    #region Fields and Properties

    [SerializeField] private TextMeshProUGUI indexText;
    //[SerializeField] private GameObject popup;
    //[SerializeField] private Sprite customerIconEmpty, customerIconNeutral, customerIconGood, customerIconBad;
    
    private ScoreManager _scoreManager;
    private bool _isOverlappingCustomerIcon;
    private RectTransform _circleReferenceTransform;

    private int _capacity;
    private int Capacity
    {
        get => _capacity;
        set
        {
            _capacity = value;
            Debug.Log($"set capacity of {value} on circle {Index}");
        }  
    }

    public int Index { get; private set; }
    public CircleEnvironment environment;
    public CircleTemperature temperature;

    #endregion

    /*private void OnEnable()
    {
        GetComponentInParent<ScrollRect>().onValueChanged.AddListener(ScaleRelatively);
    }*/

    private void Start()
    {
        Index = transform.GetSiblingIndex();
        Capacity = 9 - Index;
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _circleReferenceTransform = GameObject.FindWithTag("CircleReferenceTransform").GetComponent<RectTransform>();
        
        temperature = Index <= 3 ? CircleTemperature.Cold : CircleTemperature.Hot; 
        
        indexText.text = $"Index: {Index}, Capacity: {_capacity}\nEnvironment: {environment}\nTemperature: {temperature}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        if (Capacity == 0)
        {
            Debug.Log($"Circle {Index} at 0 capacity; cannot add another demon");
            return;
        }

        _scoreManager.AddPoints(Customer.Instance, circle: this);
        
        eventData.pointerDrag = null;
        Capacity--;
        
        GameManager.CurrentGameState = GameState.Place;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //popup.SetActive(true);
        
        if (GameManager.CurrentGameState != GameState.Drag) return;

        // if dragging, show demons?
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        //popup.SetActive(false);
        if (GameManager.CurrentGameState != GameState.Drag) return;

        return;
    }

    /*private void ScaleRelatively(Vector2 vector)
    {
        var distanceFromCenter = Mathf.Abs(transform.position.y - _circleReferenceTransform.position.y);
        var proportionalScale = distanceFromCenter * -0.00035f + 1;
        
        transform.localScale = new Vector3(proportionalScale, proportionalScale, proportionalScale);
    }*/

    /*private void OnDisable()
    {
        GetComponentInParent<ScrollRect>()?.onValueChanged.RemoveListener(ScaleRelatively);
    }*/
}
