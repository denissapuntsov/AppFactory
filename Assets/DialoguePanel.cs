using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnClickHandler();
    public static event OnClickHandler OnClick;
    public delegate void OnEndDialogueHandler(); 
    public static event OnEndDialogueHandler OnEndDialogue;
    
    [SerializeField] private RectTransform zeroTransform, exitTransform;
    private Vector3 _startPosition;
    public AudioSource audioSource;
    [SerializeField] private AudioClip panelIn, panelOut;
    public AudioClip panelSwitch;
    
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        _startPosition = GetComponent<RectTransform>().position;
    }

    public void Enter()
    {
        audioSource.PlayOneShot(panelIn);
        Sequence enterSequence = DOTween.Sequence();
        enterSequence.SetLink(gameObject);
        enterSequence
            .Append(transform.DOMove(zeroTransform.position, 0.5f))
            .OnComplete(() => GameManager.CurrentGameState = GameState.Dialogue);
    }

    public void Skip()
    {
        Debug.Log("!ShiftManager.Instance.shiftHasDialogue");
        OnEndDialogue?.Invoke();
        GameManager.CurrentGameState = GameState.Idle;
    }

    public void Exit()
    {
        audioSource.PlayOneShot(panelOut);
        Sequence exitSequence = DOTween.Sequence();
        exitSequence.SetLink(gameObject);
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
