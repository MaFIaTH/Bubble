using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using NaughtyAttributes;
using Redcode.Moroutines;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private RectTransform middleBox;
    [FormerlySerializedAs("OverlayObject")] [SerializeField] private GameObject overlayObject;
    [SerializeField] private MMF_Player screenEffectFeedback;
    [SerializeField] private AudioClip bgm;
    [Header("Debug")]
    
    [SerializeField, ReadOnly] private int offScreenRows;
    [SerializeField, ReadOnly] private List<BubbleRow> rows = new List<BubbleRow>();
    public List<BubbleRow> Rows => rows;
    [SerializeField, ReadOnly] private List<Image> backgroundImages = new List<Image>();
    public Canvas Canvas => canvas;
    public RectTransform MiddleBox => middleBox;
    public float RowSpeed => rowSpeed;
    public bool IsGameStarted = false;
    public static Action onLoopAround;
    
    private KeyValuePair<Moroutine, int> _currentSpeedCoroutine;

    private void OnEnable()
    {
        PassiveManager.Instance.OnGameStart += StartGame;
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

    void StartGame()
    {
        SpawnBackground();
        SpawnRows();
        GameManager.TotalScore = 0;
        GlobalSoundManager.Instance.PlayBGM("Gameplay", duration: 0.5f);
        IsGameStarted = true;
    }

    public void PauseGame()
    {
        if (overlayObject.activeInHierarchy)
        {
            overlayObject.SetActive(false);
            return;
        }
        overlayObject.SetActive(true);
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
            if (rectTransform.anchoredPosition.y - (height / 2) >= middleBox.anchoredPosition.y + middleBox.rect.height / 2)
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
                //row.transform.position = lastRow.transform.position + Vector3.down * rowSpacing;
                var rectTransform = row.transform as RectTransform;
                var lastRectTransform = lastRow.transform as RectTransform;
                rectTransform.anchoredPosition = lastRectTransform.anchoredPosition + Vector2.down * rowSpacing;
                row.Row = lastRow.Row + 1;
                row.SpawnBubble();
            }
            else
            {
                var row = rows[i];
                row.Row -= loopAroundThreshold;
            }
        }
        onLoopAround?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameStarted)
        {
            MoveBackground();
        }
        
    }

    public void ChangeSpeed(float multiplier, float duration, int priority)
    {
        Moroutine toDestroy = null;
        if (_currentSpeedCoroutine.Key != null && _currentSpeedCoroutine.Value <= priority)
        {
            toDestroy = _currentSpeedCoroutine.Key;
            _currentSpeedCoroutine.Key.Stop();
        }
        else if (_currentSpeedCoroutine.Key != null && _currentSpeedCoroutine.Value > priority)
        {
            return;
        }
        var animatorFeedback = screenEffectFeedback.FeedbacksList.First(x => x.Label == "EffectAnimation") as MMF_AnimatorPlayState;
        var animatorStateName = multiplier > 1 ? "Speed" : multiplier == 0 ? "Freeze" : "Slow";
        var pauseFeedback = screenEffectFeedback.FeedbacksList.First(x => x.Label == "EffectDuration") as MMF_Pause;
        if (pauseFeedback != null) pauseFeedback.PauseDuration = duration;
        if (animatorFeedback != null) animatorFeedback.StateName = animatorStateName;
        screenEffectFeedback.Initialization();
        screenEffectFeedback.PlayFeedbacks();
        var originalSpeed = rowSpeed;
        var speedCoroutine = Moroutine.Run(gameObject, ChangeSpeedCo(multiplier, duration));
        speedCoroutine.OnCompleted((x) =>
        {
            x.Destroy();
            _currentSpeedCoroutine = new KeyValuePair<Moroutine, int>(null, 0);
        });
        speedCoroutine.OnStopped((x) => rowSpeed = originalSpeed);
        toDestroy?.Destroy();
        _currentSpeedCoroutine = new KeyValuePair<Moroutine, int>(speedCoroutine, priority);
    }

    private IEnumerable ChangeSpeedCo(float multiplier, float duration)
    {
        var originalSpeed = rowSpeed;
        rowSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        rowSpeed = originalSpeed;
    }
}
