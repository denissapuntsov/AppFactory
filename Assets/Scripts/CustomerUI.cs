using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CustomerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Singleton Pattern
    
    public static CustomerUI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    #endregion
    
    public GameObject customerSelectIcon;
    private void Start()
    {
        customerSelectIcon.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Idle) return;
        GameManager.CurrentGameState = GameState.Drag;
        
        Debug.Log("Begin Drag " + gameObject.name);
        customerSelectIcon.SetActive(true);
        customerSelectIcon.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        customerSelectIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Drag) return;
        
        Debug.Log("End Drag " + gameObject.name);
        customerSelectIcon.SetActive(false);

        GameManager.CurrentGameState = GameState.Idle;
    }
}
