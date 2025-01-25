using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBomb : Bubble
{

    protected override void ActivateAbility()
    {
        // Hex grid movement vectors based on even/odd rows
        int[][] evenRowOffsets = new int[][]
        {
            new int[] { 0, -1 },  // Left
            new int[] { 0, 1 },   // Right
            new int[] { -1, 0 },  // Top-left
            new int[] { -1, 1 },  // Top-right
            new int[] { 1, 0 },   // Bottom-left
            new int[] { 1, 1 }    // Bottom-right
        };

        int[][] oddRowOffsets = new int[][]
        {
            new int[] { 0, -1 },  // Left
            new int[] { 0, 1 },   // Right
            new int[] { -1, -1 }, // Top-left
            new int[] { -1, 0 },  // Top-right
            new int[] { 1, -1 },  // Bottom-left
            new int[] { 1, 0 }    // Bottom-right
        };
        
        var bubbleCount = rowParent.Bubbles.Count;
        int[][] offsets = bubbleCount % 2 == 0 ? evenRowOffsets : oddRowOffsets;
        foreach (var offset in offsets)
        {
            var row = Row + offset[0];
            var column = Column + offset[1];
            if (row < 0 || row >= ProceduralManager.Instance.Rows.Count) continue;
            var bubbleRow = ProceduralManager.Instance.Rows[row];
            if (column < 0 || column >= bubbleRow.Bubbles.Count) continue;
            var bubble = bubbleRow.Bubbles[column];
            if (bubble != null && !bubble.Popped)
            {
                bubble.Pop(true, BubbleType.AreaBomb);
            }
        }
        
    }
}
