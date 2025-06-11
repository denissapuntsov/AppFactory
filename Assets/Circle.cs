using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private TextMeshProUGUI indexText;
    private int _index;
    private bool _isOverlappingCustomerIcon;
    
    [SerializeField] private Sprite customerIconNeutral, customerIconGood, customerIconBad;

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
        CustomerUI.Instance.customerIconParent.SetActive(false);
        
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
        //SpriteRenderer  = CustomerUI.Instance.customerSelectParent;
        //_customerImage.color = Color.green;
    }

    private void OnEnterDeep()
    {
        SpriteRenderer emotionIcon = CustomerUI.Instance.emotionIcon;
        // circles above 4 take points away, below give points; the lower the better
        if (_index < 5)
        {
            // negative
            
            emotionIcon.sprite = customerIconBad;
            return;
        }
        emotionIcon.sprite = Mathf.Abs(_index - 9) == 0 ? customerIconGood : customerIconNeutral;
    }

    private void OnEnterAvoidant()
    {
        SpriteRenderer emotionIcon = CustomerUI.Instance.emotionIcon;
        // the closer to a target circle the fewer points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);

        if (distanceFromTargetCircle > 1) emotionIcon.sprite = customerIconGood;
        else if (distanceFromTargetCircle == 1) emotionIcon.sprite = customerIconNeutral;
        else emotionIcon.sprite = customerIconBad;
    }

    private void OnEnterPrecise()
    {
        SpriteRenderer emotionIcon = CustomerUI.Instance.emotionIcon;
        // the closer to a target circle the more points given
        int distanceFromTargetCircle = Mathf.Abs(Customer.Instance.targetCircle - _index);

        if (distanceFromTargetCircle > 1) emotionIcon.sprite = customerIconBad;
        else if (distanceFromTargetCircle == 1) emotionIcon.sprite = customerIconNeutral;
        else emotionIcon.sprite = customerIconGood;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpriteRenderer emotionIcon = CustomerUI.Instance.emotionIcon;
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        emotionIcon.sprite = customerIconNeutral;
    }
}
