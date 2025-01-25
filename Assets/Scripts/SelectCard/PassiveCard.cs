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
        ChooseCard.instance.OnZoomCard+= OnZoomCard;
        ChooseCard.instance.OnUnZoomCard+= OnUnZoomCard;
        panelRectTransform = GetComponent<RectTransform>();
        OriginalPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }
    public void SetCardData(PassiveCardScriptableObject cardData)
    {
        passiveCardData = cardData;
        cardNameText.text = cardData.cardName;
        cardDecriptionText.text = cardData.cardDescription;
        costValue.text = cardData.cardCost.ToString();
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (sequence.IsActive() )
    //     {
    //         return;
    //     }
    //     panelRectTransform.SetAsLastSibling();
    //     sequence = DOTween.Sequence()
    //         .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(538f, -1069f), 0.5f))
    //         .Join(transform.DOScale(1.8f, 0.8f));
    //     
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (sequence.IsActive())
    //     {
    //         return;
    //     }
    //     sequence = DOTween.Sequence()
    //         .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(OriginalPos.x,OriginalPos.y),0.5f))
    //         .Join(transform.DOScale(1f, 0.5f));
    // }
    
    public void Selected()
    {
        if (PointManager.instance.Point >= passiveCardData.cardCost )
        {
            PointManager.instance.Point -= passiveCardData.cardCost;
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ChooseCard.instance.cardID = cardID;
        var passiveCard = this;
        ChooseCard.instance.GetPassiveCard(passiveCard);
        ChooseCard.instance.OnSelectedCard?.Invoke();
        
        
        
    }
    private void OnZoomCard()
    {
        if (this == ChooseCard.instance.lastCard )
        {
            panelRectTransform.SetAsLastSibling();
            sequence = DOTween.Sequence()
                .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(538f, -1069f), 0.5f))
                .Join(transform.DOScale(1.8f, 0.8f));
        }
        
    }
    private void OnUnZoomCard()
    {
        if (this == ChooseCard.instance.lastCard )
        {
            sequence = DOTween.Sequence()
                .Append(transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(OriginalPos.x,OriginalPos.y),0.5f))
                .Join(transform.DOScale(1f, 0.5f));
            ChooseCard.instance.cardID = CardID.None; 
            ChooseCard.instance.GetPassiveCard(null);
            ChooseCard.instance.Tapcount = 0;
        }
    }
}
