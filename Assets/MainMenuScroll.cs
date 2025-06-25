using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuScroll : MonoBehaviour
{
    private Vector3 _startPosition;

    [SerializeField] private Transform startTargetTransform, endTargetTransform;

    private Sequence _openSequence, _closeSequence;
    
    public static bool IsOpen { get; private set; }

    private void Start()
    {
        _openSequence = DOTween.Sequence();
        _closeSequence = DOTween.Sequence();
        transform.position = startTargetTransform.position;
        Open();
    }

    private void Open()
    {
        _openSequence
            .Append(transform.DOMove(endTargetTransform.position, 1f))
            .SetEase(Ease.Linear)
            .OnComplete(() => { IsOpen = true; });
    }

    public void Close()
    {
        transform.DOMove(startTargetTransform.position, 1f)
            .OnComplete(() =>
            {
                IsOpen = false;
            });
    }
}
