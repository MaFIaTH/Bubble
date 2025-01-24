using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {

        CreateHealthUI();
    }
    
    private void CreateHealthUI()
    {
        for (int i = 0; i < HealthPoint.Instance.MaxHpValue; i++)
        {
            Instantiate(heartNormalPrefab, heartPosition[i].position, Quaternion.identity, transform);
        }
    }
    
    public void TakeDamageUI()
    {
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue].GetComponent<Image>().sprite = heartDamageSprite;
        Debug.Log("Take Damage");
    }
    
    public void TakeHealUI()
    {
        heartCreatePrefab[HealthPoint.Instance.HealthPointValue - 1].GetComponent<Image>().sprite = heartNormalSprite;
        Debug.Log("Take Heal");
    }
}
