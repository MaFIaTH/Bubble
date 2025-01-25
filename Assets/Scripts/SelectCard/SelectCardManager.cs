using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectCardManager : MonoBehaviour
{
    [SerializeField] private GameObject CardUI ;
    [SerializeField] private GameObject ButtonUI;
    [SerializeField] private GameObject Background;
    [SerializeField] private PassiveCard[] passiveCards;
    [SerializeField] private PassiveCardScriptableObject[] passiveCardScriptableObjects;
    private List<PassiveCardScriptableObject> SelectedCard = new List<PassiveCardScriptableObject>(){null,null,null};
    private List<PassiveCardScriptableObject> PreviousSelectedCard = new List<PassiveCardScriptableObject>(){null,null,null};
    
    
    private void Start()
    {
        RandomCard();
    }

    public void RerollCards()
    {
        //
        RandomCard();
    }

    public void ConfirmPassive()
    {
        int index = 0;
        switch (ChooseCard.Instance.cardID)
        {
            case CardID.Card1:
                index = 0;
                break;
            case CardID.Card2:
                index = 1;
                break;
            case CardID.Card3:
                index = 2;
                break;
        }
        PassiveManager.Instance.SetPassive(SelectedCard[index].passive);
        CardUI.SetActive(false);
        ButtonUI.SetActive(false);
        Background.SetActive(false);
    }
    private void RandomCard()
    {
        SelectedCard.Clear();
        int index = 0;
        for (int i = 0; i < 3;)
        {
            int randomCard = Random.Range(0, passiveCardScriptableObjects.Length);
            if (IsDuplicate(passiveCardScriptableObjects[randomCard]))
            {
                continue;
            }
            SelectedCard.Add(passiveCardScriptableObjects[randomCard]);
            passiveCards[index].SetCardData(passiveCardScriptableObjects[randomCard]);
            index++;
            i++;
        }
        PreviousSelectedCard = new List<PassiveCardScriptableObject>(SelectedCard);
    }

    private bool IsDuplicate(PassiveCardScriptableObject card)
    {
        if (PreviousSelectedCard != null)
        {
            foreach (var selectedCard in PreviousSelectedCard)
            {
                if (selectedCard == card)
                {
                    return true;
                }
            }   
        }
        foreach (var selectedCard in SelectedCard)
        {
            if (selectedCard == card)
            {
                return true;
            }
        }
        
        return false;
    }
    

}
