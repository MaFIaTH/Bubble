using System;
using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCard : MonoSingleton<ChooseCard>, IPointerClickHandler
{
    [SerializeField] GameObject ComfirmButton;
    public CardID cardID;
    public Action OnSelectedCard;
    public Action OnZoomCard;
    public Action OnUnZoomCard;
    public int Tapcount = 0;
    private CardID lastCardID;
    public PassiveCard lastCard;

    private Image overlay;
    protected override void Awake()
    {
        base.Awake();
        overlay = GetComponent<Image>();
    }
    

    private void Start()
    {
        OnSelectedCard += TapCard;
    }
    void TapCard()
    {
        ComfirmButton.SetActive(true);
        if (cardID != lastCardID || cardID == lastCardID && Tapcount == 0 )
        {
            Tapcount = 0;
            Tapcount++;
            lastCardID = cardID;
            ((RectTransform)overlay.transform).SetAsLastSibling();
            OnZoomCard?.Invoke();
        }
        else if (cardID == lastCardID)
        {
            if (Tapcount <= 2)
            {
                return;
            }
            Tapcount++;
        }
        
        
    }

    public void GetPassiveCard(PassiveCard passiveCard)
    {
        lastCard = passiveCard;
    }
    // private bool IsMouseOverUI()
    // {
    //     return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    // }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        ((RectTransform)overlay.transform).SetAsFirstSibling();
        OnUnZoomCard?.Invoke();
        ComfirmButton.SetActive(false);
    }
}
