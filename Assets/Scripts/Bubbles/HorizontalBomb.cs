using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBomb : Bubble
{
    [SerializeField] private int radius = 2;
    protected override void ActivateAbility()
    {
        Explode();
    }

    private void Explode()
    {
        for (int i  = 1; i < radius + 1; i++)
        {
            int left = Column - i;
            int right = Column + i;
            if (left >= 0)
            {
                rowParent.Bubbles[left].Pop(true, BubbleType.HorizontalBomb);
            }
            if (right < rowParent.Bubbles.Count)
            {
                rowParent.Bubbles[right].Pop(true, BubbleType.HorizontalBomb);
            }
        }
    }
}
