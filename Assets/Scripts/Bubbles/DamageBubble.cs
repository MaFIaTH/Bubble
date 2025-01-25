using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBubble : Bubble
{
    [SerializeField] private int damage = 1;
    protected override void ActivateAbility()
    {
        float finalDamage = damage;
        PassiveManager.Instance.ApplyPassives(PassiveType.Heal, BubbleType.Damage, ref finalDamage);
        HealthPoint.Instance.ChangeHealth(-Mathf.RoundToInt(finalDamage));
    }
}
