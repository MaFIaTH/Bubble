using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Transform logo;
    [SerializeField] private Button startButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private AudioClip bgm;
    private void Start()
    {
        BopLogo();
        startButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(ToLeaderboard);
        GlobalSoundManager.Instance.PlayBGM("MainMenu", duration: 0.5f);
    }
    private void BopLogo()
    {
        logo.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void StartGame()
    {
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Gameplay, LoadSceneMode.Additive, false);
    }
    
    public void ToLeaderboard()
    {
        GameManager.TotalScore = 0;
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Leaderboard, LoadSceneMode.Additive, false);
    }
}
