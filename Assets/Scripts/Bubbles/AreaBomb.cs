using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AreaBomb : Bubble
{
    [SerializeField] private float popSize = 2f;
    [SerializeField] private float expandDuration = 0.25f;

    public override void Pop(bool fromBomb = false, BubbleType bombType = BubbleType.AreaBomb)
    {
        if (popped) return;
        if (!rowParent.OnScreen) return;
        popped = true;
        image.sprite = popSprite;
        _popSequence = DOTween.Sequence();
        _popSequence.Append(image.transform.DOScale(popSize, expandDuration));
        _popSequence.Append(image.DOFade(0, popFadeDuration));
        var finalScore = (float)score;
        if (!fromBomb)
        {
            PassiveManager.Instance.ApplyPassives(PassiveType.Score, bubbleType, ref finalScore);
        }
        else
        {
            Debug.Log("This bubbleType is " + bubbleType);
            PassiveManager.Instance.ApplyPassives(PassiveType.Score, bombType, ref finalScore);
        }
        GameManager.Instance.ChangeScore(Mathf.RoundToInt(finalScore));
        if (feedbacks) feedbacks.PlayFeedbacks();
        ActivateAbility();
        
    }
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
