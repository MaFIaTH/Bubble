using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BubbleRow : MonoBehaviour
{
    [SerializeField] private Bubble bubblePrefab;
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private int row;
    public int Row
    {
        get => row;
        set => row = value;
    }

    [SerializeField, ReadOnly] private List<Bubble> bubbles = new List<Bubble>();
    public List<Bubble> Bubbles => bubbles;
    [SerializeField, ReadOnly] private bool onScreen;
    public bool OnScreen => onScreen;
    
    public static Action onRowOffScreen;

    public void Initialize(int row, int bubblesCount)
    {
        this.row = row;
        for (int i = 0; i < bubblesCount; i++)
        {
            var bubble = Instantiate(bubblePrefab, transform);
            bubble.Initialize(row, i, this);  
            bubbles.Add(bubble);
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveRow();
        CheckOnScreen();
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
        if (rectTransform.anchoredPosition.y - (height / 2) <= ProceduralManager.Instance.Canvas.pixelRect.height / 2)
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
