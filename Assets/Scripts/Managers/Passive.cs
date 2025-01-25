using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveType
{
    DropWeight,
    Duration,
    Score,
    Speed,
    Heal
}

public enum ModifierType
{
    Additive,
    Multiplicative,
    Set,
}

[Serializable]
public class Passive
{
    [SerializeField] private PassiveType passiveType;
    public PassiveType PassiveType => passiveType;
    [SerializeField] private ModifierType modifierType;
    [SerializeField] private BubbleType affectedBubbleType;
    [SerializeField] private float value;
    
    public float ProcessValue(float originalValue, BubbleType bubbleType)
    {
        if (!IsAffected(bubbleType))
        {
            return originalValue;
        }
        switch (modifierType)
        {
            case ModifierType.Additive:
                return originalValue + value;
            case ModifierType.Multiplicative:
                return originalValue * value;
            case ModifierType.Set:
                return value;
            default:
                return originalValue;
        }
    }
    
    public bool IsAffected(BubbleType bubbleType)
    {
        return affectedBubbleType.HasFlag(bubbleType);
    }
}
