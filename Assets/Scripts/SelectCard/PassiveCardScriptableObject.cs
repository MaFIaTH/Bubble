using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Card", menuName = "Card/Passive Card")]
public class PassiveCardScriptableObject : ScriptableObject
{
   public string cardName;
   public float cardCost;
   [Multiline(2)]
   public string cardDescription;
   
}
