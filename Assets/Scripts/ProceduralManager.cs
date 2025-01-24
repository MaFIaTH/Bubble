using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralManager : MonoSingleton<ProceduralManager>
{
    [SerializeField] private BubbleRow bubbleRowPrefab;
    [SerializeField] private List<Image> repeatableBackgrounds;
    [SerializeField] private int rowsCount;
    [SerializeField] private int bubblePerRow;
    [SerializeField] private float rowSpacing;
    [SerializeField] private Vector3 firstRowPosition;
    [SerializeField] private Transform rowsParent;
    [SerializeField] private Transform backgroundParent;
    [SerializeField] private float rowSpeed;
    [SerializeField] private int loopAroundThreshold = 5;
    [SerializeField] private Canvas canvas;
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private int offScreenRows;
    [SerializeField, ReadOnly] private List<BubbleRow> rows = new List<BubbleRow>();
    [SerializeField, ReadOnly] private List<Image> backgroundImages = new List<Image>();
    public Canvas Canvas => canvas;
    public float RowSpeed => rowSpeed;

    private void OnEnable()
    {
        BubbleRow.onRowOffScreen += OnRowOffScreen;
    }

    private void OnDisable()
    {
        BubbleRow.onRowOffScreen -= OnRowOffScreen;
    }

    private void OnRowOffScreen()
    {
        offScreenRows++;
        if (offScreenRows >= loopAroundThreshold)
        {
            offScreenRows = 0;
            LoopAround();
        }
    }

    void Start()
    {
        SpawnBackground();
        SpawnRows();
    }
    
    private void SpawnRows()
    {
        for (int i = 0; i < rowsCount; i++)
        {
            var row = Instantiate(bubbleRowPrefab, rowsParent);
            row.Initialize(i, i % 2 == 0 ? bubblePerRow : bubblePerRow - 1);
            var rectTransform = row.transform as RectTransform;
            rectTransform.anchoredPosition = firstRowPosition + Vector3.down * i * rowSpacing;
            rows.Add(row);
        }
    }
    
    private void SpawnBackground()
    {
        for (int i = 0; i < repeatableBackgrounds.Count; i++)
        {
            var background = Instantiate(repeatableBackgrounds[i], backgroundParent);
            var rectTransform = background.transform as RectTransform;
            rectTransform.anchoredPosition = Vector3.down * i * rectTransform.rect.height;
            backgroundImages.Add(background);
        }
    }
    
    private void MoveBackground()
    {
        for (int i = 0; i < backgroundImages.Count; i++)
        {
            var background = backgroundImages[i];
            var rectTransform = background.transform as RectTransform;
            rectTransform.anchoredPosition += Vector2.up * (rowSpeed * Time.deltaTime);
            var height = rectTransform.rect.height;
            if (rectTransform.anchoredPosition.y - (height / 2) >= canvas.pixelRect.height / 2)
            {
                rectTransform.anchoredPosition -= Vector2.up * (rectTransform.rect.height * backgroundImages.Count);
            }
        }
    }

    private void LoopAround()
    {
        for (int i = rowsCount - 1; i >= 0; i--)
        {
            if (i < loopAroundThreshold)
            {
                var row = rows[i];
                var lastRow = rows[rows.Count - 1];
                rows.RemoveAt(i);
                rows.Add(row);
                row.transform.SetAsLastSibling();
                row.transform.position = lastRow.transform.position + Vector3.down * rowSpacing;
                row.Row = lastRow.Row + 1;
            }
            else
            {
                var row = rows[i];
                row.Row -= loopAroundThreshold;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBackground();
    }
}
