using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform zeroTransform, exitTransform;
    private RectTransform startTransform;
    
    private void OnEnable()
    {
        GameManager.OnEnter += Enter;
    }

    private void Enter()
    {
        Sequence enterSequence = DOTween.Sequence();
        enterSequence
            .Append(transform.DOMove(zeroTransform.position, 0.5f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Dialogue);
    }

    private void Exit()
    {
        Sequence exitSequence = DOTween.Sequence();
        exitSequence 
            .Append(transform.DOMove(exitTransform.position, 0.5f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Idle);
    }

    private void OnDisable()
    {
        GameManager.OnEnter -= Enter;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.CurrentGameState != GameState.Dialogue) return;
        // TODO: either skip to next dialogue line or transition to game
        Exit();
    }
}
