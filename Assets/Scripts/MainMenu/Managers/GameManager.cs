using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using Redcode.Moroutines;
using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text totalPointText;
    
    [SerializeField] private float scoreMultiplier = 1;
    [SerializeField] private float gameTimer = 120;
    [SerializeField] private MMF_Player freezeFrameFeedback;
    [SerializeField] private int testScore;
    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private MMF_Player screenEffectFeedback;
    private bool is15FirstTime = true;
    private Sequence _sequence;
    [SerializeField] public GameObject totalPointObject;
    public static int TotalScore;
    public static int TotalScoreValue
    {
        get => TotalScore;
        set => TotalScore = value;
    }

    Moroutine _scoreMultiplierCoroutine;
    // Start is called before the first frame update

    [Button("Save Score")]

    private void SaveScore()
    {
        TotalScore = testScore;
    }
    void Start()
    {
        scoreText.text = score.ToString();
        totalPointObject.SetActive(true);
    }
 
    private void Update()
    {
        totalPointText.text = TotalScoreValue.ToString();
        if (ProceduralManager.Instance.IsGameStarted)
        {
            UpdateTimer();
        }
    }
    
    private void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 15 && is15FirstTime )
        {
            _sequence = DOTween.Sequence()
                .Append(timerText.GetComponent<RectTransform>().DOScale(1.4f, 0.1f)).OnComplete(() =>
                timerText.GetComponent<RectTransform>().DOScale(1, 0.1f).SetLoops(-1, LoopType.Yoyo))
                .Join(timerText.DOColor(Color.red, 0.2f));
            is15FirstTime = false;
        }
        // float to MM:SS format
        if (gameTimer <= 0)
        {
            _sequence.Kill(); // WTF? Why it's not working
            gameTimer = 0;
            //Go to game ledderboard
            GameOver();
        }
        timerText.text = $"{Mathf.Floor(gameTimer / 60):00}:{(gameTimer % 60):00}";
    }

    public void GameOver()
    {
        ProceduralManager.Instance.IsGameStarted = false;
        TotalScore = score;
        gameCanvasGroup.interactable = false;
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Leaderboard, LoadSceneMode.Additive, false);
    }

    public void ChangeScore(int value)
    {
        scoreText.GetComponent<RectTransform>().DOScale(1.3f, 0.2f).OnComplete(() => scoreText.GetComponent<RectTransform>().DOScale(1, 0.2f));
        score += Mathf.RoundToInt(value * scoreMultiplier);
        TotalScore = score;
        scoreText.text = score.ToString();
    }
    
    public void ChangeScoreMultiplier(float value, float duration)
    {
        _scoreMultiplierCoroutine?.Stop();
        var toDestroy = _scoreMultiplierCoroutine;
        if (value > 1)
        {
            var animatorFeedback = screenEffectFeedback.FeedbacksList.First(x => x.Label == "EffectAnimation") as MMF_AnimatorPlayState;
            var pauseFeedback = screenEffectFeedback.FeedbacksList.First(x => x.Label == "EffectDuration") as MMF_Pause;
            if (pauseFeedback != null) pauseFeedback.PauseDuration = duration;
            if (animatorFeedback != null) animatorFeedback.StateName = "Golden Hour";
            screenEffectFeedback.Initialization();
            screenEffectFeedback.PlayFeedbacks();
        }
        _scoreMultiplierCoroutine = Moroutine.Run(gameObject, ChangeScoreMultiplierCoroutine(value, duration));
        _scoreMultiplierCoroutine.OnCompleted(x => x?.Destroy());
        toDestroy?.Destroy();
    }
    
    public void FreezeFrame()
    {
        freezeFrameFeedback?.PlayFeedbacks();
    }
    
    private IEnumerable ChangeScoreMultiplierCoroutine(float value, float duration)
    {
        var originalMultiplier = scoreMultiplier;
        scoreMultiplier = value;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = originalMultiplier;
    }
    
    public void ToMainMenu()
    {
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.MainMenu, LoadSceneMode.Additive, false);
    }
    
    public void RestartGame()
    {
        TotalScore = 0;
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Gameplay, LoadSceneMode.Single, false);
    }
}
