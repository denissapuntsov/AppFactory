using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnClickHandler();
    public static event OnClickHandler OnClick;
    public delegate void OnEndDialogueHandler(); 
    public static event OnEndDialogueHandler OnEndDialogue;
    
    [SerializeField] private RectTransform zeroTransform, exitTransform;
    private Vector3 _startPosition;
    
    private void OnEnable()
    {
        GameManager.OnEnter += Enter;
        _startPosition = GetComponent<RectTransform>().position;
    }

    private void Enter()
    {
        Sequence enterSequence = DOTween.Sequence();
        enterSequence
            .Append(transform.DOMove(zeroTransform.position, 0.5f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Dialogue);
    }

    public void Exit()
    {
        Sequence exitSequence = DOTween.Sequence();
        exitSequence 
            .Append(transform.DOMove(exitTransform.position, 0.5f))
            .OnComplete(() =>
            {
                transform.position = _startPosition;
                OnEndDialogue?.Invoke();
                GameManager.CurrentGameState = GameState.Idle;
            });
    }

    private void OnDisable()
    {
        GameManager.OnEnter -= Enter;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState == GameState.Dialogue) OnClick?.Invoke();
    }
}
