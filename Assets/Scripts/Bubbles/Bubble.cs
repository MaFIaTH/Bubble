using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Flags]
public enum BubbleType
{
    None = 0,
    Normal = 1 << 0,
    HorizontalBomb = 1 << 1,
    DiagonalBomb = 1 << 2,
    AreaBomb = 1 << 3,
    Damage = 1 << 4,
    Heal = 1 << 5,
    HyperSpeed = 1 << 6,
    SuperSlow = 1 << 7,
    GoldenHour = 1 << 8,
    Nuke = 1 << 9,
    TimeStop = 1 << 10,
    BadNormal = 1 << 11,
    MysteryBox = 1 << 12
}
[RequireComponent(typeof(Image))]
public abstract class Bubble : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected BubbleType bubbleType;
    [SerializeField] private float dropWeight = 1;
    [SerializeField] protected int score;
    [SerializeField] protected MMF_Player feedbacks;
    [SerializeField] protected Sprite popSprite;
    [SerializeField] protected float popFadeDuration = 0.5f;
    protected Sequence _popSequence;

    public float DropWeight
    {
        get
        {
            float finalDropWeight = dropWeight;
            PassiveManager.Instance.ApplyPassives(PassiveType.DropWeight, bubbleType, ref finalDropWeight);
            return finalDropWeight;
        }
    }
    
    [Header("Debug")] 
    [SerializeField, ReadOnly] private int row;
    public int Row => row;
    [SerializeField, ReadOnly] private int column;
    public int Column => column;
    [SerializeField, ReadOnly] protected BubbleRow rowParent;
    public BubbleRow RowParent => rowParent;
    [SerializeField, ReadOnly] protected bool popped;
    
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

    public virtual void Pop(bool fromBomb = false, BubbleType bombType = BubbleType.AreaBomb)
    {
        if (popped) return;
        if (!rowParent.OnScreen) return;
        popped = true;
        image.sprite = popSprite;
        _popSequence = DOTween.Sequence();
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

    protected abstract void ActivateAbility();
    public void OnPointerClick(PointerEventData eventData)
    {
        Pop();
    }
}
