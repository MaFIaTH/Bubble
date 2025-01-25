using System;
using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class PassiveManager : PersistentMonoSingleton<PassiveManager>
{
    [SerializeField] private List<Passive> passives = new List<Passive>();
    public List<Passive> Passives => passives;
    public Action OnGameStart;

    public void ApplyPassives(PassiveType passiveType, BubbleType bubbleType, ref float value)
    {
        foreach (var passive in passives)
        {
            if (passive.PassiveType == passiveType)
            {
                value = passive.ProcessValue(value, bubbleType);
            }
        }
    }

    public void SetPassive(Passive newPassives)
    {
        //test
        passives.Add(newPassives) ;
    }
}
