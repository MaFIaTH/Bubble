using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveCard : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler ,IPointerClickHandler
{
    private PassiveCardScriptableObject passiveCardData;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDecriptionText;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TextMeshProUGUI costValue;

    private void Update()
    {
        
    }
    public void SetCardData(PassiveCardScriptableObject cardData)
    {
        passiveCardData = cardData;
        cardNameText.text = cardData.cardName;
        cardDecriptionText.text = cardData.cardDescription;
        costValue.text = cardData.cardCost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.3f,0.5f);
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f,0.5f);
        Debug.Log("Exits");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        Selected();
    }

    public void Selected()
    {
        PointManager.instance.Point -= passiveCardData.cardCost;
    }
}
