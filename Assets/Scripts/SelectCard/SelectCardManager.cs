using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectCardManager : MonoBehaviour
{
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
