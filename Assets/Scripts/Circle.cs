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
    
    private ScoreManager _scoreManager;
    private bool _isOverlappingCustomerIcon;
    private RectTransform _circleReferenceTransform;

    public int Index { get; private set; }
    public CircleEnvironment environment;
    public CircleTemperature temperature;

    #endregion

    private void Start()
    {
        Index = transform.GetSiblingIndex();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _circleReferenceTransform = GameObject.FindWithTag("CircleReferenceTransform").GetComponent<RectTransform>();
        
        temperature = Index <= 3 ? CircleTemperature.Cold : CircleTemperature.Hot; 
        
        indexText.text = $"Index: {Index},\nEnvironment: {environment}\nTemperature: {temperature}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        eventData.pointerDrag = null;
        _scoreManager.AddPoints(Customer.Instance, circle: this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //popup.SetActive(true);
        
        if (GameManager.CurrentGameState != GameState.Drag) return;

        CircleManager.CurrentCircle = this;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        //popup.SetActive(false);
        if (GameManager.CurrentGameState != GameState.Drag) return;

        CircleManager.CurrentCircle = null;
        return;
    }
}
