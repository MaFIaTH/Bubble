using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    private static HealthPoint _instance;
    
    [SerializeField] private int minHp = 3;
    public int MinHpValue => minHp;
    [SerializeField] private int maxHp = 5;
    
    private int maxNowHp = 3;
    public int MaxNowHpValue => maxNowHp;
    
    private int healthPoint = 3;
    public int HealthPointValue => healthPoint;
    
    
    private int takeDamagePoint = 1;
    public int TakeDamagePoint => takeDamagePoint;
    
    public static HealthPoint Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("HealthPoint is null");
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void TakeDamage()
    {
        if (healthPoint <= 0)
            return;
   
        healthPoint -= takeDamagePoint;
        
    }
    
    private void TakeHeal()
    {
        if (healthPoint > maxNowHp)
            return;
        
        healthPoint++;
    }
    
    public void TestHealHp()
    {
        TakeHeal();
        HealthUI.Instance.TakeHealUI();
    }
    
    public void TestDamageHp()
    {
        TakeDamage();
        HealthUI.Instance.TakeDamageUI();
    }
    
    public void TestUpMaxHealth()
    {
        if (maxNowHp >= maxHp)
            return;
        
        maxNowHp++;
        TakeHeal();
        HealthUI.Instance.CreateHealthUI();

        Debug.Log("Max Health Up " + maxNowHp);
    }
    
    public void TestResetMaxHp()
    {
        maxNowHp = minHp;
        TakeDamage();
        HealthUI.Instance.ResetHealthUI();
        
        Debug.Log("Max Health Reset " + maxNowHp);
    }
}
