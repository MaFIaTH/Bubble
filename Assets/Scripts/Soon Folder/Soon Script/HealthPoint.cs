using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    private static HealthPoint _instance;
    
    private int maxHp = 3;
    public int MaxHpValue => maxHp;
    
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
        //HealthPointTest();
    }
    
    private void TakeDamage()
    {
        if (healthPoint <= 0)
            return;
   
        healthPoint -= takeDamagePoint;
        HealthUI.Instance.TakeDamageUI();
        
    }
    
    private void TakeHeal()
    {
        if (healthPoint >= maxHp)
            return;
        
        healthPoint++;
        HealthUI.Instance.TakeHealUI();
    }
    
    /*
    private void HealthPointTest()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage();
        }
        if (Input.GetMouseButtonDown(1))
        {
            TakeHeal();
        }
    }
    */
    
    public void TestUpMaxHealth()
    {
        if (maxHp >= 5)
            return;
        
        maxHp++;
        TakeHeal();
        Debug.Log("Max Health Up" + maxHp);
    }
    
    public void TestDownMaxHealth()
    {
        maxHp--;
        Debug.Log("Max Health Down" + maxHp);
    }
}
