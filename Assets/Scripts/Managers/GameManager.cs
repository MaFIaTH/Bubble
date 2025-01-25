using System.Collections;
using System.Collections.Generic;
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

    Moroutine _scoreMultiplierCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
        // float to MM:SS format
        timerText.text = $"{Mathf.Floor(gameTimer / 60):00}:{(gameTimer % 60):00}";
        if (gameTimer <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void ChangeScore(int value)
    {
        score += Mathf.RoundToInt(value * scoreMultiplier);
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
    
    private IEnumerable ChangeScoreMultiplierCoroutine(float value, float duration)
    {
        var originalMultiplier = scoreMultiplier;
        scoreMultiplier = value;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = originalMultiplier;
    }
}
