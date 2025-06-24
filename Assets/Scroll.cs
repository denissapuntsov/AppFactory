using System;
using DG.Tweening;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private RectTransform startScrollReference, endScrollReference;
    [SerializeField] private int anchoredStartPositionY; // 1155
    private void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, anchoredStartPositionY);
        transform.position = startScrollReference.position;
        DialoguePanel.OnEndDialogue += () => GetComponent<RectTransform>().DOMove(endValue: endScrollReference.position, duration: 1f);
    }
}
