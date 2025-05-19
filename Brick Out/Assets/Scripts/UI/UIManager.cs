using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Level UI")]
    [SerializeField] private Text _levelText;

    [Header("Level Complete Panel")]
    [SerializeField] private GameObject _levelCompletePanel;
    [SerializeField] private Text _levelScoreText;
    [SerializeField] private Text _totalScoreText;
    [SerializeField] private Text _levelCompletedText;
    [SerializeField] private Button _nextLevelButton;
    
    private int _currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
    }

    private void OnNextLevelClicked()
    {
        Time.timeScale = 1f; // Возобновляем игру
        _levelCompletePanel.SetActive(false);
        GameManager.Instance.StartNextLevel();
        ScoreManager.Instance.ResetCurrentLevelScore();
    }

    public void ShowLevelStart(int level)
    {
        _levelText.text = $"LEVEL {level}";
        _currentLevel = level;
    }

    public void ShowLevelComplete(int levelScore, int totalScore)
    {
        Time.timeScale = 0f; // Пауза игры
        _levelCompletePanel.SetActive(true);
        _levelScoreText.text = $"{levelScore}";
        _totalScoreText.text = $"{totalScore}";
        _levelCompletedText.text = $"Level {_currentLevel} completed!";
    }

    private void OnDestroy()
    {
        _nextLevelButton.onClick.RemoveAllListeners();
    }
}