using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
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
                var heartPrefab = Instantiate(heartNormalPrefab, transform).GetComponent<Image>();
                heartPrefab.GetComponent<RectTransform>().DOScale(1.3f, 0.2f).OnComplete(() => heartPrefab.GetComponent<RectTransform>().DOScale(1, 0.2f));
                heartCreatePrefab.Add(heartPrefab);
                
            }
        }
        else if (HealthPoint.Instance.MaxHpNowValue > HealthPoint.Instance.DefaultMaxHpValue)
        {
            var heartPrefab = Instantiate(heartNormalPrefab, transform).GetComponent<Image>();
            heartPrefab.GetComponent<RectTransform>().DOScale(1.3f, 0.2f).OnComplete(() => heartPrefab.GetComponent<RectTransform>().DOScale(1, 0.2f));
            heartCreatePrefab.Add(heartPrefab);
            
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
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].transform.DOScale(1.3f, 0.2f).OnComplete(() => heartCreatePrefab[HealthPoint.Instance.HealthPointValue].GetComponent<RectTransform>().DOScale(1, 0.2f));
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].sprite = heartDamageSprite;
        
    }
    
    //Normal Heart Change
    public void TakeHealUI()
    {
        
        
        
        
        if (HealthPoint.Instance.HealthPointValue > HealthPoint.Instance.MaxHpNowValue)
            return;
        if (PassiveManager.Instance.Passives.Count == 0)
        {
            heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].sprite = heartNormalSprite;
            heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].transform.DOScale(1.3f, 0.2f).OnComplete(() => heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].GetComponent<RectTransform>().DOScale(1, 0.2f));
            return;
        }
        if (PassiveManager.Instance.Passives[0].PassiveType == PassiveType.Heal)
        {
            for (int i = 1; i < HealthPoint.Instance.HealthPointValue; i++)
            {
                var heartPrefab = heartCreatePrefab[HealthPoint.Instance.HealthPointValue - i];
                if (heartPrefab.sprite == heartDamageSprite)
                {
                    heartPrefab.sprite = heartNormalSprite;
                    heartPrefab.transform.DOScale(1.3f, 0.2f).OnComplete(() => heartPrefab.GetComponent<RectTransform>().DOScale(1, 0.2f));
                }
            }
            return;
        }
        
    }
        
        
        
    

    public void nextUpgradePassive()
    {
        PassiveUpgradeUI.SetActive(true);
        PassiveUpgradeButton.SetActive(true);
        HpUI.SetActive(false);
    }
}
