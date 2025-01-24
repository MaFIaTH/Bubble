using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HyperSpeed : Bubble
{
    [SerializeField] private int speedPriority = 1;
    public int SpeedPriority => speedPriority;
    [SerializeField] private float speedMultiplier = 2;
    [SerializeField] private float duration = 5f;
    protected override void ActivateAbility()
    {
        ProceduralManager.Instance.ChangeSpeed(speedMultiplier, duration, speedPriority);
    }
}
