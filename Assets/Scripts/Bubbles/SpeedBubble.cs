using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpeedBubble : Bubble
{
    [SerializeField] private int speedPriority = 1;
    public int SpeedPriority => speedPriority;
    [SerializeField] private float speedMultiplier = 2;
    [SerializeField] private float duration = 5f;
    protected override void ActivateAbility()
    {
        float finalSpeedMultiplier = speedMultiplier;
        PassiveManager.Instance.ApplyPassives(PassiveType.Speed, bubbleType, ref finalSpeedMultiplier);
        float finalDuration = duration;
        PassiveManager.Instance.ApplyPassives(PassiveType.Duration, bubbleType, ref finalDuration);
        ProceduralManager.Instance.ChangeSpeed(finalSpeedMultiplier, finalDuration, speedPriority);
    }
}
