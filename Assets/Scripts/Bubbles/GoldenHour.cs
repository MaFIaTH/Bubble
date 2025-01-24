using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenHour : Bubble
{
    [SerializeField] private float scoreMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ActivateAbility()
    {
        GameManager.Instance.ChangeScoreMultiplier(scoreMultiplier, duration);
    }
}
