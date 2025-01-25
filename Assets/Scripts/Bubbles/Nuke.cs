using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Bubble
{
    protected override void ActivateAbility()
    {
        NukeEverything();
    }

    private void NukeEverything()
    {
        GameManager.Instance.FreezeFrame();
        ProceduralManager.Instance.Rows.ForEach(row =>
        {
            row.Bubbles.ForEach(bubble =>
            {
                if (bubble != null)
                {
                    bubble.Pop(true, BubbleType.Nuke);
                }
            });
        });
    }
}
