using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum BubbleType
{
    Normal,
    HorizontalBomb,
    AreaBomb
}
[RequireComponent(typeof(Image))]
public class Bubble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BubbleType bubbleType;
    
    [Header("Debug")] 
    [SerializeField, ReadOnly] private int row;
    public int Row => row;
    [SerializeField, ReadOnly] private int column;
    public int Column => column;
    [SerializeField, ReadOnly] private BubbleRow rowParent;
    public BubbleRow RowParent => rowParent;
    [SerializeField, ReadOnly] private bool popped;
    public bool Popped => popped;
    
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(int row, int column, BubbleRow rowParent)
    {
        this.row = row;
        this.column = column;
        this.rowParent = rowParent;
    }

    public void Pop()
    {
        if (popped) return;
        popped = true;
        image.color = Color.clear;
        ActivateAbility();
    }

    public virtual void ActivateAbility()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Pop();
    }
}
