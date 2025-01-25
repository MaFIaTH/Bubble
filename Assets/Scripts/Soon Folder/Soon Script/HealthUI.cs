using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    static HealthUI _instance;
    
    
    [SerializeField] private Transform[] heartPosition;
    
    [SerializeField] private GameObject heartNormalPrefab;

    [SerializeField] private Sprite heartNormalSprite;
    [SerializeField] private Sprite heartDamageSprite;
    
    
    [SerializeField] public List<GameObject> heartCreatePrefab = new List<GameObject>();
    
    public static HealthUI Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("HealthUI is null");
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
        CreateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void CreateHealthUI()
    {
        if (HealthPoint.Instance.MaxNowHpValue == HealthPoint.Instance.MinHpValue)
        {
            for (int i = 0; i < HealthPoint.Instance.MaxNowHpValue; i++)
            {
                heartCreatePrefab.Add(Instantiate(heartNormalPrefab, heartPosition[i].position, Quaternion.identity, transform));
            }
        }
        else if (HealthPoint.Instance.MaxNowHpValue > HealthPoint.Instance.MinHpValue)
        {
            heartCreatePrefab.Add(Instantiate(heartNormalPrefab, heartPosition[HealthPoint.Instance.MaxNowHpValue -1].position, Quaternion.identity, transform));
        }
        
        
        
    }
    
    // รอแก้ ไม่สามารถลบได้
    public void ResetHealthUI()
    {
        for (int i = HealthPoint.Instance.MaxNowHpValue; i > HealthPoint.Instance.MinHpValue; i--)
        {
            if (i > HealthPoint.Instance.MinHpValue)
            {
                Destroy(heartCreatePrefab[i -1]);
            }
        }
    }
    
    //Break Heart Change
    public void TakeDamageUI()
    {
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].GetComponent<Image>().sprite = heartDamageSprite;
        Debug.Log("Take Damage");
    }
    
    //Normal Heart Change
    public void TakeHealUI()
    {
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].GetComponent<Image>().sprite = heartNormalSprite;
        Debug.Log("Take Heal");
    }
}
