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
        float finalScoreMultiplier = scoreMultiplier;
        PassiveManager.Instance.ApplyPassives(PassiveType.Score, bubbleType, ref finalScoreMultiplier);
        float finalDuration = duration;
        PassiveManager.Instance.ApplyPassives(PassiveType.Duration, bubbleType, ref finalDuration);
        GameManager.Instance.ChangeScoreMultiplier(finalScoreMultiplier, finalDuration);
    }
}
