using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject infoFieldUI;
    [SerializeField] private RectTransform infoFieldUITarget;
    [SerializeField] private float duration = 0.2f;

    private Vector3 _defaultPosition;
    private CanvasGroup _canvasGroup;

    private Sequence _popupSequence;

    private void Start()
    {
        _defaultPosition = infoFieldUI.transform.position;
        _canvasGroup = GetComponent<CanvasGroup>();
        infoFieldUI.transform.position = infoFieldUITarget.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Idle) return;
        
        _canvasGroup.alpha = 0.5f;
        
        infoFieldUI.transform.DOMove(_defaultPosition, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        infoFieldUI.transform.DOMove(infoFieldUITarget.position, duration);
    }
}
