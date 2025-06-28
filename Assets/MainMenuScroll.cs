using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuScroll : MonoBehaviour
{
    private Vector3 _startPosition;

    [SerializeField] private Transform startTargetTransform, endTargetTransform;

    private Sequence _openSequence, _closeSequence;
    
    public bool IsOpen { get; private set; }

    private void Start()
    {
        _closeSequence = DOTween.Sequence();
        transform.position = startTargetTransform.position;
        Open();
    }

    public void Open()
    {
        _openSequence = DOTween.Sequence();
        _openSequence
            .Append(transform.DOMove(endTargetTransform.position, 0.3f))
            .SetEase(Ease.Linear)
            .OnComplete(() => { IsOpen = true; });
    }

    public void Close()
    {
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
