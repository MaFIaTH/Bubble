using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoSingleton<HealthUI>
{
    [SerializeField] private GameObject heartNormalPrefab;
    [SerializeField] public Sprite heartNormalSprite;
    [SerializeField] private Sprite heartDamageSprite;
    [SerializeField] public List<Image> heartCreatePrefab = new List<Image>();

    [SerializeField] private GameObject PassiveUpgradeUI;
    [SerializeField] private GameObject PassiveUpgradeButton;
    [SerializeField] private GameObject HpUI;
    // Start is called before the first frame update
    void Start()
    {
        CreateHealthUI();
    }

    public void CreateHealthUI()
    {
        if (HealthPoint.Instance.MaxHpNowValue == HealthPoint.Instance.DefaultMaxHpValue)
        {
            for (int i = 0; i < HealthPoint.Instance.MaxHpNowValue; i++)
            {
                heartCreatePrefab.Add(Instantiate(heartNormalPrefab, transform).GetComponent<Image>());
            }
        }
        else if (HealthPoint.Instance.MaxHpNowValue > HealthPoint.Instance.DefaultMaxHpValue)
        {
            heartCreatePrefab.Add(Instantiate(heartNormalPrefab, transform).GetComponent<Image>());
        }
    }
    
    public void ResetHealthUI()
    {
        for (int i = HealthPoint.Instance.MaxHpNowValue; i > HealthPoint.Instance.DefaultMaxHpValue; i--)
        {
            if (i > HealthPoint.Instance.DefaultMaxHpValue)
            {
                Destroy(heartCreatePrefab[i -1].gameObject);
                heartCreatePrefab.RemoveAt(i -1);
            }
        }
    }
    
    public void RestoreHp()
    {
        for (int i = 0; i < HealthPoint.Instance.MaxHpNowValue; i++)
        {
            heartCreatePrefab[i].sprite = heartNormalSprite;
        }
    }
    
    //Break Heart Change
    public void TakeDamageUI()
    {
        if (HealthPoint.Instance.HealthPointValue < 0)
            return;
        HealthPoint.Instance.OnDamaged?.Invoke();
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].sprite = heartDamageSprite;
    }
    
    //Normal Heart Change
    public void TakeHealUI()
    {
        Debug.Log(PassiveManager.Instance.Passives[0].PassiveType );
        if (HealthPoint.Instance.HealthPointValue > HealthPoint.Instance.MaxHpNowValue)
            return;
        
        if (PassiveManager.Instance.Passives[0].PassiveType == PassiveType.Heal)
        {
            for (int i = 1; i < HealthPoint.Instance.HealthPointValue; i++)
            {
                Debug.Log(i);
                heartCreatePrefab[HealthPoint.Instance.HealthPointValue - i].sprite = heartNormalSprite;
            }
            return;
        }
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].sprite = heartNormalSprite;
    }

    public void nextUpgradePassive()
    {
        PassiveUpgradeUI.SetActive(true);
        PassiveUpgradeButton.SetActive(true);
        HpUI.SetActive(false);
    }
}
