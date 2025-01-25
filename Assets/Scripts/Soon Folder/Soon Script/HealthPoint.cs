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
    [SerializeField] private int maximumMaxHp = 6;
    [SerializeField] private Image[] nonHp;
    private int maxHpNow = 3;
    public int MaxHpNowValue => maxHpNow;
    private int levelHp = 0;
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
    public void IncreaseMaxHp()
    {
        if (maxHpNow < maximumMaxHp)
        {
            maxHpNow++;
            healthPoint++;
            HealthUI.Instance.CreateHealthUI();
        }
    }
    
    public void TryUpgrade()
    {
          
        switch (levelHp)
        {
            case 0:
                Debug.Log("Upgrade"); 
                Upgrade(50000,0);
                break;
            case 1:
                Upgrade(100000,1);
                break;
            case 2:
                Upgrade(150000,2);
                break;
            default:
                Debug.Log("HasNoEnoghUpgrade");
                //  button.interactable = false;
                break;
                
        }
    }

    public void Upgrade(int point,int index)
    {
        if (point <= GameManager.TotalScore)
        {
            IncreaseMaxHp();
            GameManager.TotalScore -= point;
            UpdateHpUI(index);
            if (levelHp < 2){
                 levelHp++;
            }
            
        }
    }
    private void UpdateHpUI(int index)
    {
        nonHp[index].sprite = HealthUI.Instance.heartNormalSprite;
    }
}