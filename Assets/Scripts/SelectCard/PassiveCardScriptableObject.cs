using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Passive Card", menuName = "Card/Passive Card")]
public class PassiveCardScriptableObject : ScriptableObject
{
   public Sprite BubbleImage;  
   [Multiline(2)]
   public string cardName;
   public int cardCost;
   [Multiline(3)]
   public string cardDescription;
   public Passive passive;
}
