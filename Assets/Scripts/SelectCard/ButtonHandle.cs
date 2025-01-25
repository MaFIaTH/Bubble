using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonHandle : MonoBehaviour
{
    [SerializeField] Vector2 zoomPos;
    private Sequence sequence;
    private Vector2 OriginalPos;
    private void Start()
    {
        ChooseCard.instance.OnZoomCard += MoveButton;
        ChooseCard.instance.OnUnZoomCard += MoveButtonBack;
        OriginalPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }
    void MoveButton()
    {
        sequence = DOTween.Sequence()
            .Append(transform.GetComponent<RectTransform>().DOAnchorPos(zoomPos, 0.5f))
            .Join(transform.DOScale(1.5f, 0.5f));
    }
    void MoveButtonBack()
    {
        sequence = DOTween.Sequence()
            .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(OriginalPos.x,OriginalPos.y), 0.5f))
            .Join(transform.DOScale(1f, 0.5f));
    }
}
