using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private TextMeshProUGUI indexText;
    private int _index;
    private bool _isOverlappingCustomerIcon;

    private void OnEnable()
    {
        return;
    }

    private void Awake()
    {
        _index = transform.GetSiblingIndex();
        indexText.text = _index.ToString();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        eventData.pointerDrag = null;
        CustomerUI.Instance.customerSelectIcon.SetActive(false);
        
        GameManager.CurrentGameState = GameState.Place;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
        // any circle gives equal points
        RawImage _customerImage = CustomerUI.Instance.customerSelectIcon.GetComponent<RawImage>();
        _customerImage.color = Color.green;
    }

    private void OnEnterDeep()
    {
        RawImage _customerImage = CustomerUI.Instance.customerSelectIcon.GetComponent<RawImage>();
        // circles above 4 take points away, below give points; the lower the better
        if (_index < 5)
        {
            // negative
            
            _customerImage.color = Color.red;
            return;
        }
        _customerImage.color = Mathf.Abs(_index - 9) == 0 ? Color.green : Color.yellow;
    }

    private void OnEnterAvoidant()
    {
        RawImage _customerImage = CustomerUI.Instance.customerSelectIcon.GetComponent<RawImage>();
        // the closer to a target circle the fewer points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);

        if (distanceFromTargetCircle > 1) _customerImage.color = Color.green;
        else if (distanceFromTargetCircle == 1) _customerImage.color = Color.yellow;
        else _customerImage.color = Color.red;
    }

    private void OnEnterPrecise()
    {
        RawImage _customerImage = CustomerUI.Instance.customerSelectIcon.GetComponent<RawImage>();
        // the closer to a target circle the more points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);
        
        if (distanceFromTargetCircle > 1) _customerImage.color = Color.red;
        else if (distanceFromTargetCircle == 1) _customerImage.color = Color.yellow;
        else _customerImage.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RawImage _customerImage = CustomerUI.Instance.customerSelectIcon.GetComponent<RawImage>();
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        _customerImage.color = Color.grey;
    }
}
