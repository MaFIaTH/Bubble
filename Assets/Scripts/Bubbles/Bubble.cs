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
    DiagonalBomb,
    AreaBomb,
    Damage,
    Heal,
    HyperSpeed,
    SuperSlow,
    GoldenHour,
    Nuke,
    TimeStop,
    BadNormal,
    MysteryBox
}
[RequireComponent(typeof(Image))]
public abstract class Bubble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BubbleType bubbleType;
    [SerializeField] private float dropWeight = 1;
    [SerializeField] private int score;
    public float DropWeight => dropWeight;
    
    [Header("Debug")] 
    [SerializeField, ReadOnly] private int row;
    public int Row => row;
    [SerializeField, ReadOnly] private int column;
    public int Column => column;
    [SerializeField, ReadOnly] protected BubbleRow rowParent;
    public BubbleRow RowParent => rowParent;
    [SerializeField, ReadOnly] private bool popped;
    
    public bool Popped => popped;
    
    protected Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ProceduralManager.onLoopAround += OnLoopAround;
    }
    
    private void OnDisable()
    {
        ProceduralManager.onLoopAround -= OnLoopAround;
    }

    protected virtual void OnLoopAround()
    {
        row = rowParent.Row;
    }

    public virtual void Initialize(int row, int column, BubbleRow rowParent)
    {
        this.row = row;
        this.column = column;
        this.rowParent = rowParent;
    }

    public virtual void Pop()
    {
        if (popped) return;
        if (!rowParent.OnScreen) return;
        popped = true;
        image.color = Color.clear;
        GameManager.Instance.ChangeScore(score);
        ActivateAbility();
    }

    protected abstract void ActivateAbility();

    public void OnPointerClick(PointerEventData eventData)
    {
        Pop();
    }
}
