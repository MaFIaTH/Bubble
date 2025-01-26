using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private float scoreMultiplier = 1;
    [SerializeField] private float gameTimer = 120;
    [SerializeField] private MMF_Player freezeFrameFeedback;
    [SerializeField] private int testScore;
    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private MMF_Player screenEffectFeedback;
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
    }
 
    private void Update()
    {
        if (ProceduralManager.Instance.IsGameStarted)
        {
            UpdateTimer();
        }
    }
    
    private void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
        // float to MM:SS format
        if (gameTimer <= 0)
        {
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
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Gameplay, LoadSceneMode.Single, false);
    }
}
