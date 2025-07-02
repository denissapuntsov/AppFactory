using System;
using DG.Tweening;
using UnityEditor.Build;
using UnityEngine;

public class FreePlayScroll : MonoBehaviour, IScroll
{
    [SerializeField] private Transform startTargetTransform, endTargetTransform;
    private AudioSource _audioSource;
    private Sequence _openSequence;
    
    public bool IsOpen { get; private set; }

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        DialoguePanel.OnEndDialogue += Open;
    }

    private void Awake()
    {
        transform.position = startTargetTransform.position;
    }

    public void Open()
    {
        _audioSource.Play();
        
        _openSequence = DOTween.Sequence();
        _openSequence.SetLink(gameObject);
        _openSequence
            .Append(transform.DOMove(endTargetTransform.position, 1f))
            .SetEase(Ease.Linear)
            .OnComplete(() => { IsOpen = true; });
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        DialoguePanel.OnEndDialogue -= Open;
    }
}
