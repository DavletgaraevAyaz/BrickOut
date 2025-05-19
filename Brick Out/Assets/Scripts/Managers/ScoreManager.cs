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

    public int CurrentScore => _playerData.TotalScore;
    public int LevelScore => _playerData.CurrentLevel;

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

    public void AddScore(int amount)
    {
        _levelScore += amount;
        _currentLevelScore += amount;
        _playerData.AddScore(amount);
        OnScoreChanged?.Invoke(_playerData.TotalScore);
        OnLevelScoreChanged?.Invoke(_levelScore);
        Debug.Log($"Score added: {amount}. Total: {_playerData.TotalScore}");
    }

    private void LoadPlayerData()
    {
        _playerData = PlayerData.LoadData();
    }

    public void ResetCurrentLevelScore()
    {
        _currentLevelScore = 0;
        OnScoreChanged?.Invoke(_playerData.TotalScore);
        OnLevelScoreChanged?.Invoke(_currentLevelScore);
    }

    public void CompleteLevel(int levelIndex)
    {
        _playerData.CurrentLevel = levelIndex;
        _playerData.UnlockNextLevel();
    }

    public int GetLevelScore => _currentLevelScore;

    public void ResetLevelScore()
    {
        _levelScore = 0;
    }
}