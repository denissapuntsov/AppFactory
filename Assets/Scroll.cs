using System;
using DG.Tweening;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private RectTransform scrollReference;
    private void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 1155);
        float referenceEndPointY = scrollReference.position.y;
        DialoguePanel.OnEndDialogue += () => GetComponent<RectTransform>().DOMoveY(endValue: referenceEndPointY, duration: 1f);
    }
}
