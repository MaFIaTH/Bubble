using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using Redcode.Moroutines;
using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float scoreMultiplier = 1;
    [SerializeField] private float gameTimer = 120;
    [SerializeField] private MMF_Player freezeFrameFeedback;
    [SerializeField] private int testScore;
    public static int TotalScore;
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
        timerText.text = $"{Mathf.Floor(gameTimer / 60):00}:{(gameTimer % 60):00}";
        if (gameTimer <= 0)
        {
            //Go to game ledderboard
            ProceduralManager.Instance.IsGameStarted = false;
            Debug.Log("Game Over");
        }
    }

    public void ChangeScore(int value)
    {
        score += Mathf.RoundToInt(value * scoreMultiplier);
        TotalScore += score;
        scoreText.text = score.ToString();
    }
    
    public void ChangeScoreMultiplier(float value, float duration)
    {
        _scoreMultiplierCoroutine?.Stop();
        var toDestroy = _scoreMultiplierCoroutine;
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
}
