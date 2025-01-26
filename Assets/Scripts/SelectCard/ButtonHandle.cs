using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonHandle : MonoBehaviour
{
    [SerializeField] Vector2 zoomPos;
    [SerializeField] private GameObject RerollText;
    private Sequence sequence;
    private Vector2 OriginalPos;
    private void Start()
    {
        ChooseCard.Instance.OnZoomCard += MoveButton;
        ChooseCard.Instance.OnUnZoomCard += MoveButtonBack;
        OriginalPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }
    void MoveButton()
    {
        if (RerollText != null)
        {
            RerollText.SetActive(false);
        }
        sequence = DOTween.Sequence()
            .Append(transform.GetComponent<RectTransform>().DOAnchorPos(zoomPos, 0.5f))
            .Join(transform.DOScale(1.3f, 0.5f));
    }
    void MoveButtonBack()
    {
        if (RerollText != null)
        {
            RerollText.SetActive(true);
        }
        sequence = DOTween.Sequence()
            .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(OriginalPos.x,OriginalPos.y), 0.5f))
            .Join(transform.DOScale(1f, 0.5f));
    }
}
