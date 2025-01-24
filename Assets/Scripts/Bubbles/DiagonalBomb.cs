using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public enum DiagonalDirection
{
    Left,
    Right
}
public class DiagonalBomb : Bubble
{
    [SerializeField, ReadOnly] private DiagonalDirection direction;
    [SerializeField] private Sprite leftDiagonalBomb;
    [SerializeField] private Sprite rightDiagonalBomb;
    [SerializeField] private int radius = 2;

    public override void Initialize(int row, int column, BubbleRow rowParent)
    {
        base.Initialize(row, column, rowParent);
        direction = Random.Range(0, 2) == 0 ? DiagonalDirection.Right : DiagonalDirection.Left;
        image.color = direction == DiagonalDirection.Left ? Color.red : Color.blue;
    }
    protected override void ActivateAbility()
    {
        Explode();
    }

    private void Explode()
    {
        var rowCount = ProceduralManager.Instance.Rows.Count;
        bool topReached = false;
        bool bottomReached = false;
        bool centerEven = rowParent.Bubbles.Count % 2 == 0;
        for (int i = 1; i < radius + 1; i++)
        {
            var nextTopRow = Row - i;
            var nextBottomRow = Row + i;
            if (nextTopRow < 0 || nextTopRow >= rowCount) topReached = true;
            if (nextBottomRow < 0 || nextBottomRow >= rowCount) bottomReached = true;
            if (topReached && bottomReached) break;
            
            int topPopColumn;
            if (direction == DiagonalDirection.Right)
            {
                topPopColumn = Column + (centerEven ? Mathf.CeilToInt(i / 2f) : Mathf.FloorToInt(i / 2f));
            }
            else
            {
                topPopColumn = Column - (centerEven ? Mathf.FloorToInt(i / 2f) : Mathf.CeilToInt(i / 2f));
            }
            int bottomPopColumn; 
            if (direction == DiagonalDirection.Right)
            {
                bottomPopColumn = Column - (centerEven ? Mathf.FloorToInt(i / 2f) : Mathf.CeilToInt(i / 2f));
            }
            else
            {
                bottomPopColumn = Column + (centerEven ? Mathf.CeilToInt(i / 2f) : Mathf.FloorToInt(i / 2f));
            }
            
            if (!topReached)
            {
                var nextTop = ProceduralManager.Instance.Rows[nextTopRow];
                if (topPopColumn >= 0 && topPopColumn < nextTop.Bubbles.Count)
                {
                    var topBubble = nextTop.Bubbles[topPopColumn];
                    topBubble.Pop();
                }
            }

            if (!bottomReached)
            {
                var nextBottom = ProceduralManager.Instance.Rows[nextBottomRow];
                if (bottomPopColumn >= 0 && bottomPopColumn < nextBottom.Bubbles.Count)
                {
                    var bottomBubble = nextBottom.Bubbles[bottomPopColumn];
                    bottomBubble.Pop();
                }
            }
        }
    }
}
