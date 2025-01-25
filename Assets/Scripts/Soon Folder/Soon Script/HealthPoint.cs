using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI levelText;
    private int maxHpNow = 3;
    public int[] levelCost;
    [SerializeField] private TextMeshProUGUI levelCostText;
    public Action OnDamaged;
    public int MaxHpNowValue => maxHpNow;
    private int levelHp = 0;
    private int healthPoint = 3;
    public int HealthPointValue => healthPoint;

    private void Start()
    {
        OnDamaged += IsDie;
        levelText.text = levelHp.ToString();
        levelCostText.text = levelCost[0].ToString();
    }
    private void IsDie()
    {
        if (healthPoint <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
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
                Debug.Log("Upgrade1"); 
                Upgrade(levelCost[0],0);
                break;
            case 1:
                Debug.Log("Upgrade2");  
                Upgrade(levelCost[1],1);
                break;
            case 2:
                Debug.Log("Upgrade3"); 
                Upgrade(levelCost[2],2);
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
            if (index > 2)
            {
                levelCostText.text = "Max Upgrade";
                return;
            }
            IncreaseMaxHp();
            GameManager.TotalScore -= point;
            UpdateHpUI(index);
            if (levelHp < 2){
                
                 levelHp++;
                 Debug.Log(levelHp);
                 levelText.text = (levelHp).ToString();
                 if (index <= 1)
                 {
                     levelCostText.text = levelCost[index+1].ToString();
                 }
                
                 return;
            }
            
            levelText.text = (levelHp + 1).ToString();
            
            
        }
    }
    private void UpdateHpUI(int index)
    {
        nonHp[index].sprite = HealthUI.Instance.heartNormalSprite;
    }
}