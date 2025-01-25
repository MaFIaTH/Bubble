using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Redcode.Extensions;
using UnityEngine;

public class MysteryBox : Bubble
{
    protected override void ActivateAbility()
    {
        RandomBubble();
    }

    private void RandomBubble()
    {
        var allBubbles = rowParent.BubblePrefabs.Where(x => x.GetType() != typeof(MysteryBox)).ToList();
        var randomBubble = allBubbles.GetRandomElement();
        var bubble = Instantiate(randomBubble, transform.position, Quaternion.identity);
        bubble.Initialize(Row, Column, rowParent);
        bubble.Pop();
    }
}
