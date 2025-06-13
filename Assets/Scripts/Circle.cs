using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private GameObject popup;
    
    private int _index;
    private bool _isOverlappingCustomerIcon;
    private Image _emotionIcon;

    private int _capacity;
    
    public int Capacity
    {
        get => _capacity;
        private set
        {
            _capacity = value;
            indexText.text = $"Index: {_index}\nCapacity: {_capacity}";
            Debug.Log($"set capacity of {value} on circle {_index}");
        }
    }

    [SerializeField] private Sprite customerIconEmpty, customerIconNeutral, customerIconGood, customerIconBad;

    private void Awake()
    {
        _index = transform.GetSiblingIndex();
        Capacity = 9 - _index;
    }

    private void Start()
    {
        _emotionIcon = CustomerUI.Instance.emotionIcon;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        if (Capacity == 0)
        {
            Debug.Log($"Circle {_index} at 0 capacity; cannot add another demon");
            return;
        }
        
        eventData.pointerDrag = null;
        CustomerUI.Instance.customerIconParent.SetActive(false);
        Capacity--; 
        GameManager.CurrentGameState = GameState.Place;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //popup.SetActive(true);
        
        if (GameManager.CurrentGameState != GameState.Drag) return;

        switch (Customer.Instance.currentType)
        {
               case CustomerType.Common:
                   OnEnterCommon();
                   break;
               case CustomerType.Deep:
                   OnEnterDeep();
                   break;
               case CustomerType.Avoidant:
                   OnEnterAvoidant();
                   break;
               case CustomerType.Precise:
                   OnEnterPrecise();
                   break;
        }
    }

    private void OnEnterCommon()
    {
        _emotionIcon.sprite = customerIconGood;
    }

    private void OnEnterDeep()
    {
        // circles above 4 take points away, below give points; the lower the better
        if (_index < 5)
        {
            // negative
            
            _emotionIcon.sprite = customerIconBad;
            return;
        }
        _emotionIcon.sprite = Mathf.Abs(_index - 8) == 0 ? customerIconGood : customerIconNeutral;
    }

    private void OnEnterAvoidant()
    {
        // the closer to a target circle the fewer points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);

        if (distanceFromTargetCircle > 1) _emotionIcon.sprite = customerIconGood;
        else if (distanceFromTargetCircle == 1) _emotionIcon.sprite = customerIconNeutral;
        else _emotionIcon.sprite = customerIconBad;
    }

    private void OnEnterPrecise()
    {
        // the closer to a target circle the more points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);

        if (distanceFromTargetCircle > 1) _emotionIcon.sprite = customerIconBad;
        else if (distanceFromTargetCircle == 1) _emotionIcon.sprite = customerIconNeutral;
        else _emotionIcon.sprite = customerIconGood;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //popup.SetActive(false);
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        _emotionIcon.sprite = customerIconEmpty;
    }
}
