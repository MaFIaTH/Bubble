using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ChooseCard : Card
{
    
    public static ChooseCard instance;
    [SerializeField] GameObject ComfirmButton;
    public Action OnSelectedCard;
    public Action OnZoomCard;
    public Action OnUnZoomCard;
    public int Tapcount = 0;
    private CardID lastCardID;
    public PassiveCard lastCard;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            OnUnZoomCard?.Invoke();
            ComfirmButton.SetActive(false);
        }
    }

    private void Start()
    {
        OnSelectedCard += TapCard;
    }
    void TapCard()
    {
        ComfirmButton.SetActive(true);
        Debug.Log(cardID );
        if ( cardID != lastCardID || cardID == lastCardID && Tapcount == 0 )
        {
            Debug.Log("Tapped");
            Tapcount = 0;
            Tapcount++;
            lastCardID = cardID;
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
        Debug.Log(Tapcount);
        
    }

    public void GetPassiveCard(PassiveCard passiveCard)
    {
        lastCard = passiveCard;
    }
    private bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
