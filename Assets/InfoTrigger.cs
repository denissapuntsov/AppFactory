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

    private Vector3 _defaultPosition;
    private CanvasGroup _canvasGroup;
    
    private bool _canRewind = false;

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

        Debug.Log("lkjjhgfjgk");
        _canvasGroup.alpha = 0.5f;
        
        //if (_canRewind) return;
        
        infoFieldUI.transform.DOMove(_defaultPosition, 0.2f).OnComplete(() => _canRewind = true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        StartCoroutine(routine: WaitForTween());
    }

    private IEnumerator WaitForTween()
    {
        yield return new WaitUntil(() => _canRewind);
        infoFieldUI.transform.DOMove(infoFieldUITarget.position, 0.2f).OnComplete(() => _canRewind = false);
    }
}
