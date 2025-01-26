using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectCardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardUI ;
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private GameObject background;
    [SerializeField] private PassiveCard[] passiveCards;
    [SerializeField] private PassiveCardScriptableObject[] passiveCardScriptableObjects;
    [SerializeField]private bool isTest = false;
    private List<PassiveCardScriptableObject> selectedCard = new List<PassiveCardScriptableObject>(){null,null,null};
    private List<PassiveCardScriptableObject> previousSelectedCard = new List<PassiveCardScriptableObject>(){null,null,null};
    
    
    
    private void Start()
    {
        RandomCard();
    }

    public void RerollCards()
    {
        RandomCard();
    }
    public bool IsCanBuyCard(int index)
    {
        if (GameManager.TotalScore >= selectedCard[index].cardCost)
        {
            GameManager.TotalScore -= selectedCard[index].cardCost;
            Debug.Log(GameManager.TotalScore);
            return true;
        }
        Debug.LogWarning("you're cost is not enoght");
        return false;

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

        if (!isTest)
        {
            if (!IsCanBuyCard(index)){ return;}
        }
        PassiveManager.Instance.SetPassive(selectedCard[index].passive);
        cardUI.SetActive(false);
        buttonUI.SetActive(false);
        background.SetActive(false);
    }
    private void RandomCard()
    {
        selectedCard.Clear();
        int index = 0;
        for (int i = 0; i < 3;)
        {
            int randomCard = Random.Range(0, passiveCardScriptableObjects.Length);
            if (IsDuplicate(passiveCardScriptableObjects[randomCard]))
            {
                continue;
            }
            selectedCard.Add(passiveCardScriptableObjects[randomCard]);
            passiveCards[index].SetCardData(passiveCardScriptableObjects[randomCard]);
            index++;
            i++;
        }
        previousSelectedCard = new List<PassiveCardScriptableObject>(selectedCard);
    }

    private bool IsDuplicate(PassiveCardScriptableObject card)
    {
        if (previousSelectedCard != null)
        {
            foreach (var selectedCard in previousSelectedCard)
            {
                if (selectedCard == card)
                {
                    return true;
                }
            }   
        }
        foreach (var selectedCard in selectedCard)
        {
            if (selectedCard == card)
            {
                return true;
            }
        }
        return false;
    }
    

}
