using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dan.Main;
using Dan.Models;
using DG.Tweening;
using NaughtyAttributes;
using Redcode.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text newHighScoreText;
    
    [Header("Top 3")]
    [SerializeField] private PlayerEntry[] top3Entries;

    [Header("Notification")]
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private float notificationDuration = 3f;
    
    [Header("Leaderboard")]
    [SerializeField] private TMP_Text leaderboardTitle;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private PlayerEntry playerEntryPrefab;
    [SerializeField] private TMP_Text maxEntriesText;
    [SerializeField] private int maxEntries = 50;
    [SerializeField] private Color selfEntryColor;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button personalButton;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    //[SerializeField] private Button retryButton;
    
    // [Header("Scene Transition")]
    // [SerializeField] private GameObject canvas;
    // [SerializeField] private Vector3 canvasSlideDistance;
    
    [Header("Debug")]
    [SerializeField] private string testUsername;
    [SerializeField] private int testScore;
    
    private List<PlayerEntry> _playerEntries = new List<PlayerEntry>();
    private bool _fromMainMenu;
    private PlayerEntry _selfEntry;
    private Tween _notificationTween;
    private Coroutine _loadMainMenuSceneCoroutine;
    private AudioSource _bgmAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        // canvas.transform.localPosition -= canvasSlideDistance;
        // Vector3 targetPosition = canvas.transform.localPosition + canvasSlideDistance;
        // canvas.transform.DOLocalMove(targetPosition, 3f).SetEase(Ease.OutQuart).OnComplete(() =>
        // {
        //     // if (SceneManager.GetSceneByName(SceneNames.Game.ToString()).isLoaded)
        //     //     SceneManager.UnloadSceneAsync(SceneNames.Game.ToString());
        //     // if (SceneManager.GetSceneByName(SceneNames.MainMenu.ToString()).isLoaded)
        //     // {
        //     //     SceneManager.UnloadSceneAsync(SceneNames.MainMenu.ToString());
        //     //     _fromMainMenu = true;
        //     // }
        //     
        // });
        ShowScore();
        Debug.Log($"TSV: {GameManager.TotalScoreValue}");
        //SoundManager.Instance.PlayBGM(BGMTypes.Leaderboard, out _bgmAudioSource);
        //SoundManager.Instance.PlaySoundFX(SoundFXTypes.Celebrate, out _);
        submitButton.onClick.AddListener(SubmitButton);
        personalButton.onClick.AddListener(PersonalButton);
        refreshButton.onClick.AddListener(RefreshButton);
        deleteButton.onClick.AddListener(DeleteButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
        retryButton.onClick.AddListener(RetryButton);
        //leaderboardTitle.gameObject.SetActive(false);
        newHighScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        scoreText.text = GameManager.TotalScoreValue.ToString("N0");
        scoreText.transform.localScale = Vector3.zero;
        scoreText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        maxEntriesText.text = $"Top {maxEntries}";
        notificationText.transform.localScale = Vector3.zero;
        notificationText.text = "";
        top3Entries.ForEach(a => a.gameObject.SetActive(false));
        GetEntries();
        if (!SceneManager.GetSceneByName(SceneTypes.MainMenu.ToString()).isLoaded) return;
        scoreText.gameObject.SetActive(false);
        leaderboardTitle.gameObject.SetActive(true);
        usernameInput.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
    }

    private void ShowScore()
    {
        if (_fromMainMenu) return;
        CheckNewHighScore();
    }
    
    private void CheckNewHighScore()
    {
        GetPersonalEntry(true);
    }
    
    [Button("Test Get Entries")]
    public void GetEntries(bool scrollToSelf = false)
    {
        ActivateSubmission(false);
        ResetEntriesView();
        Leaderboards.ProjectBubble.GetEntries(a => OnGetEntries(a, scrollToSelf), OnGetEntriesFailed);
    }
    
    public void RefreshButton()
    {
        GetEntries();
    }

    public void UploadEntry(string username, int score)
    {
        ActivateSubmission(false);
        ResetEntriesView();
        Leaderboards.ProjectBubble.UploadNewEntry(username, score, OnUploadEntrySuccess, OnUploadEntryFailed);
    }

    [Button("Upload Test Entry")]
    private void UploadTestEntry()
    {
        UploadEntry(testUsername, testScore);
    }
    
    public void SubmitButton()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            ShowNotification("Please enter a username");
            return;
        }
        UploadEntry(usernameInput.text, GameManager.TotalScoreValue);
    }

    public void DeleteEntry()
    {
        ActivateSubmission(false);
        ResetEntriesView();
        Leaderboards.ProjectBubble.DeleteEntry(OnDeleteEntrySuccess, OnDeleteEntryFailed);
    }
    
    [Button("Delete Test Entry")]
    private void DeleteTestEntry()
    {
        DeleteEntry();
    }

    public void DeleteButton()
    {
        DeleteEntry();
    }
    
    public void GetPersonalEntry(bool checkNewHighScore = false)
    {
        if (!checkNewHighScore)
        {
            ActivateSubmission(false);
            ResetEntriesView();
        }
        Leaderboards.ProjectBubble.GetPersonalEntry((a) => OnPersonalEntryLoaded(a, checkNewHighScore), OnGetEntriesFailed);
    }
    
    public void PersonalButton()
    {
        GetPersonalEntry();
    }
    
    private void ResetEntriesView()
    {
        foreach (var playerEntry in _playerEntries)
        {
            Destroy(playerEntry.gameObject);
        }
        _playerEntries.Clear();
    }
    
    private void ActivateSubmission(bool activate)
    {
        loadingText.gameObject.SetActive(!activate);
        usernameInput.interactable = activate;
        submitButton.interactable = activate;
        deleteButton.interactable = activate;
        refreshButton.interactable = activate;
        personalButton.interactable = activate;
    }
    
    private void OnGetEntries(Entry[] entries, bool scrollToSelf = false)
    {
        ActivateSubmission(true);
        //Top 3 entries
        for (var i = 0; i < 3; i++)
        {
            if (i < entries.Length)
            {
                top3Entries[i].SetEntry(entries[i].Rank, entries[i].Username, entries[i].Score);
                top3Entries[i].gameObject.SetActive(true);
                top3Entries[i].ResetTextColor();
                if (entries[i].IsMine())
                {
                    _selfEntry = top3Entries[i];
                    top3Entries[i].SetTextColor(selfEntryColor);
                }
            }
            else
            {
                top3Entries[i].gameObject.SetActive(false);
            }
        }
        //The rest of the entries
        for (var i = 3; i < maxEntries; i++)
        {
            if (i >= entries.Length) break;
            CreateEntryDisplay(entries[i]);
        }
        if (!scrollToSelf) return;
        //scroll to self entry
        if (_selfEntry && _playerEntries.Contains(_selfEntry))
        {
            Canvas.ForceUpdateCanvases();
            float normalizedPosition =
                1f - ((_playerEntries.FindIndex(a => _selfEntry == a) + 1) / (float)_playerEntries.Count);
            Debug.Log($"Normalized Position: {normalizedPosition}");
            scrollRect.verticalNormalizedPosition = normalizedPosition;
            //SoundManager.Instance.PlaySoundFX(SoundFXTypes.NameTag, out _);
        }
        else if (top3Entries.Contains(_selfEntry))
        {
            ShowNotification($"Congrats on the top 3!");
        }
        else
        {
            ShowNotification($"You are not on the top {maxEntries} list...");
        }
    }
    
    private void CreateEntryDisplay(Entry entry)
    {
        PlayerEntry newPlayerEntry = Instantiate(playerEntryPrefab, scrollRect.content);
        newPlayerEntry.SetEntry(entry.Rank, entry.Username, entry.Score);
        _playerEntries.Add(newPlayerEntry);
        bool isMine = entry.IsMine();
        if (isMine)
        {
            _selfEntry = newPlayerEntry;
            newPlayerEntry.SetTextColor(selfEntryColor);
        }
    }

    private void OnGetEntriesFailed(string error)
    {
        ActivateSubmission(true);
        Debug.LogWarning(error);
        ShowNotification(error);
    }
    
    private void OnUploadEntrySuccess(bool success)
    {
        if (success)
        {
            GetEntries(true);
        }
    }
    
    private void OnUploadEntryFailed(string error)
    {
        ActivateSubmission(true);
        Debug.LogWarning(error);
        ShowNotification(error);
    }
    
    private void OnDeleteEntrySuccess(bool success)
    {
        if (success)
        {
            GetEntries();
        }
    }
    
    private void OnDeleteEntryFailed(string error)
    {
        ActivateSubmission(true);
        Debug.LogWarning(error);
        ShowNotification($"{error}Deleting entry failed!");
    }
    
    private void OnPersonalEntryLoaded(Entry entry, bool checkNewHighScore = false)
    {
        ActivateSubmission(true);
        if (checkNewHighScore)
        {
            int score = GameManager.TotalScoreValue;
            if (score < entry.Score) return;
            newHighScoreText.gameObject.SetActive(true);
            string newHighScoreString = score <= 0 ? "Seriously?" : "New High!";
            newHighScoreText.text = newHighScoreString;
            newHighScoreText.transform.localScale = Vector3.zero;
            newHighScoreText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            //if (score > 0) SoundManager.Instance.PlaySoundFX(SoundFXTypes.NewHighScore, out _);
            return;
        }
        if (entry.Rank == 0)
        {
            ShowNotification("No personal entry found");
            return;
        }
        ResetEntriesView();
        CreateEntryDisplay(entry);
        //SoundManager.Instance.PlaySoundFX(SoundFXTypes.NameTag, out _);
    }
    
    private void ShowNotification(string message)
    {
        if (_notificationTween.IsActive()) return;
        notificationText.text = message;
        _notificationTween = notificationText.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
        {
            _notificationTween = DOVirtual.DelayedCall(notificationDuration, () =>
            {
                _notificationTween = notificationText.transform.DOScale(Vector3.zero, 0.2f);
            });
        });
    }
    
    [Button("Test Notification")]
    private void TestNotification()
    {
        ShowNotification("This is a test notification");
    }
    
    public void MainMenuButton()
    {
        // if (SceneManager.sceneCount > 1) return;
        // if (_loadMainMenuSceneCoroutine != null) return;
        // // SoundManager.Instance.PlaySoundFX(SoundFXTypes.SceneTransition, out _);
        // // SoundManager.Instance.StopSound(_bgmAudioSource);
        // GameManager.TotalScoreValue = 0;
        // _loadMainMenuSceneCoroutine = StartCoroutine(LoadMainMenuScene());
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.MainMenu, LoadSceneMode.Additive, false);
    }
    public void RetryButton()
    {
        //if (SceneManager.sceneCount > 1) return;
        // LoadSceneManager.Instance.Retry = true;
        // LoadSceneManager.Instance.Score = 0;
        // SoundManager.Instance.StopSound(_bgmAudioSource);
        // SceneManager.LoadScene(SceneNames.Game.ToString());
        SceneManagerPersistent.Instance.LoadNextScene(SceneTypes.Gameplay, LoadSceneMode.Additive, false);
    }
}
