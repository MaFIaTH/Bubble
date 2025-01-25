using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class HealthPoint : MonoSingleton<HealthPoint>
{
    [SerializeField] private int defaultMaxHp = 3;
    public int DefaultMaxHpValue => defaultMaxHp;
    [SerializeField] private int maximumMaxHp = 5;

    private int maxHpNow = 3;
    public int MaxHpNowValue => maxHpNow;

    private int healthPoint = 3;
    public int HealthPointValue => healthPoint;
    
    public void ChangeHealth(int value)
    {
        healthPoint += value;
        if (healthPoint > maxHpNow)
            healthPoint = maxHpNow;
        if (healthPoint < 0)
            healthPoint = 0;
        
        if (value > 0)
        {
            HealthUI.Instance.TakeHealUI();
        }
        else
        {
            HealthUI.Instance.TakeDamageUI();
        }
    }
}