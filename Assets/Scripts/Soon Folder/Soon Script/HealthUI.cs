using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoSingleton<HealthUI>
{
    [SerializeField] private GameObject heartNormalPrefab;
    [SerializeField] private Sprite heartNormalSprite;
    [SerializeField] private Sprite heartDamageSprite;
    [SerializeField] public List<Image> heartCreatePrefab = new List<Image>();

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
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].sprite = heartDamageSprite;
    }
    
    //Normal Heart Change
    public void TakeHealUI()
    {
        if (HealthPoint.Instance.HealthPointValue > HealthPoint.Instance.MaxHpNowValue)
            return;
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].sprite = heartNormalSprite;
    }
}
