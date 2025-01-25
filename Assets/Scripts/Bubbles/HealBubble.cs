using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBubble : Bubble
{
    [SerializeField] private int healAmount = 1;
    protected override void ActivateAbility()
    {
        float finalHeal = healAmount;
        PassiveManager.Instance.ApplyPassives(PassiveType.Heal, BubbleType.Heal, ref finalHeal);
        HealthPoint.Instance.ChangeHealth(Mathf.RoundToInt(finalHeal));
    }
}
