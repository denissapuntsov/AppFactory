using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuScroll : MonoBehaviour, IScroll
{
    private Vector3 _startPosition;

    [SerializeField] private Transform startTargetTransform, endTargetTransform;

    private Sequence _openSequence, _closeSequence;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip open, close;
    
    public bool IsOpen { get; private set; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _closeSequence = DOTween.Sequence();
        transform.position = startTargetTransform.position;
        Open();
    }

    public void Open()
    {
        _audioSource.PlayOneShot(open);
        _openSequence = DOTween.Sequence();
        _openSequence
            .Append(transform.DOMove(endTargetTransform.position, 0.3f))
            .SetEase(Ease.Linear)
            .OnComplete(() => { IsOpen = true; });
    }

    public void Close()
    {
        _audioSource.PlayOneShot(close);
        _closeSequence = DOTween.Sequence();
        _closeSequence
            .Append(transform.DOMove(startTargetTransform.position, 0.3f))
            .AppendInterval(0.3f)
            .OnComplete(() =>
            {
                IsOpen = false;
            });
    }
}
