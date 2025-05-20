using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public event Action<int> OnScoreChanged;
    public event Action<int> OnLevelScoreChanged;

    private PlayerData _playerData;
    private int _currentLevelScore;
    private int _levelScore;
    private int _totalScore;
    public int LevelScore => _playerData.CurrentLevel;
    public int TotalScore => _totalScore;

    public void CurrentScore()
    {
        StartCoroutine(ApiService.Instance.GetUserScore((coins, error) =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"{error}");
                return;
            }
            _totalScore = coins;
            Debug.Log(_totalScore);
        }));

    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        CurrentScore();
    }

    public void AddScore(int amount)
    {
        StartCoroutine(ApiService.Instance.AddCoins(amount , (coins, error) =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"{error}");
                return;
            }
            _totalScore = coins;
            OnScoreChanged?.Invoke(_totalScore);
        }));
        _levelScore += amount;
        _currentLevelScore += amount;
        OnLevelScoreChanged?.Invoke(_levelScore);
    }

    private void LoadPlayerData()
    {
        _playerData = PlayerData.LoadData(ApiService.Instance.UserId());
    }

    public void ResetCurrentLevelScore()
    {
        _currentLevelScore = 0;
       OnScoreChanged?.Invoke(_totalScore);
        OnLevelScoreChanged?.Invoke(_currentLevelScore);
    }

    public void CompleteLevel(int levelIndex)
    {
        _playerData.CurrentLevel = levelIndex;
        _playerData.UnlockNextLevel(ApiService.Instance.UserId());
    }

    public int GetLevelScore => _currentLevelScore;

    public void ResetLevelScore()
    {
        _levelScore = 0;
    }
}