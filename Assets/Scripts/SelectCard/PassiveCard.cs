using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveCard : Card  ,IPointerClickHandler
{
    private PassiveCardScriptableObject passiveCardData;
    public RectTransform panelRectTransform;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDecriptionText;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TextMeshProUGUI costValue;
    private Vector2 OriginalPos;
    private Sequence sequence;
    
    private void Start()
    {
        ChooseCard.Instance.OnZoomCard+= OnZoomCard;
        ChooseCard.Instance.OnUnZoomCard+= OnUnZoomCard;
        panelRectTransform = GetComponent<RectTransform>();
        OriginalPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }
    public void SetCardData(PassiveCardScriptableObject cardData)
    {
        cardIcon.sprite = cardData.BubbleImage;
        passiveCardData = cardData;
        cardNameText.text = cardData.cardName;
        cardDecriptionText.text = cardData.cardDescription;
        costValue.text = cardData.cardCost.ToString();
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ChooseCard.Instance.cardID = cardID;
        var passiveCard = this;
        ChooseCard.Instance.GetPassiveCard(passiveCard);
        ChooseCard.Instance.OnSelectedCard?.Invoke();
    }
    private void OnZoomCard()
    {
        if (this == ChooseCard.Instance.lastCard )
        {
            panelRectTransform.SetAsLastSibling();
            if (sequence.IsActive()) sequence.Kill();
            sequence = DOTween.Sequence()
                .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(538f, -1069f), 0.5f))
                .Join(transform.DOScale(1.8f, 0.8f));
        }
        
    }
    private void OnUnZoomCard()
    {
        if (this == ChooseCard.Instance.lastCard )
        {
            if (sequence.IsActive()) sequence.Kill();
            sequence = DOTween.Sequence()
                .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(OriginalPos.x,OriginalPos.y),0.5f))
                .Join(transform.DOScale(1f, 0.5f));
            ChooseCard.Instance.cardID = CardID.None; 
            ChooseCard.Instance.GetPassiveCard(null);
            ChooseCard.Instance.Tapcount = 0;
        }
    }
}
