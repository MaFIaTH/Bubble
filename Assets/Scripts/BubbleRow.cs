using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BubbleRow : MonoBehaviour
{
    [SerializeField] private Bubble[] bubblePrefabs;
    public Bubble[] BubblePrefabs => bubblePrefabs;

    [Header("Debug")]
    [SerializeField, NaughtyAttributes.ReadOnly] private int row;
    public int Row
    {
        get => row;
        set => row = value;
    }

    [SerializeField, NaughtyAttributes.ReadOnly] private List<Bubble> bubbles = new List<Bubble>();
    public List<Bubble> Bubbles => bubbles;
    [SerializeField, NaughtyAttributes.ReadOnly] private bool onScreen;
    public bool OnScreen => onScreen;
    
    private int _bubblesCount;
    
    public static Action onRowOffScreen;

    public void Initialize(int row, int bubblesCount)
    {
        this.row = row;
        _bubblesCount = bubblesCount;
        SpawnBubble();
    }
    
    public void SpawnBubble()
    {
        bubbles.ForEach(b => Destroy(b.gameObject));
        bubbles.Clear();
        for (int i = 0; i < _bubblesCount; i++)
        {
            var bubble = Instantiate(GetRandomBubble(), transform);
            bubble.Initialize(row, i, this);  
            bubbles.Add(bubble);
        };
    }
    
    
    private Bubble GetRandomBubble()
    {
        var combinedWeight = bubblePrefabs.Sum(b => b.DropWeight);
        var randomValue = UnityEngine.Random.Range(0, combinedWeight);

        float cumulativeWeight = 0;
        foreach (var bubble in bubblePrefabs)
        {
            cumulativeWeight += bubble.DropWeight;
            if (randomValue < cumulativeWeight)
            {
                return bubble;
            }
        }

        return bubblePrefabs.Last(); // Fallback (should never hit)
    }


    // Update is called once per frame
    void Update()
    {
        if (ProceduralManager.Instance.IsGameStarted)
        {
            MoveRow();
            CheckOnScreen();
        }
        
    }

    private void MoveRow()
    {
        var rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition += Vector2.up * (ProceduralManager.Instance.RowSpeed * Time.deltaTime);
    }
    
    private void CheckOnScreen()
    {
        var rectTransform = transform as RectTransform;
        var height = rectTransform.rect.height;
        if (rectTransform.anchoredPosition.y - (height / 2) <= ProceduralManager.Instance.Canvas.pixelRect.height / 2 && 
            rectTransform.anchoredPosition.y + (height / 2) >= -ProceduralManager.Instance.Canvas.pixelRect.height / 2)
        {
            onScreen = true;
        }
        else
        {
            if (onScreen)
            {
                onRowOffScreen?.Invoke();
            }
            onScreen = false;
        }
    }
}
