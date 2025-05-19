using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private BallSpawner _ballSpawner;
    [SerializeField] private BlockSystem _blockSystem;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private LevelGenerator _levelGenerator;

    private int _currentLevel = 1;
    private int _currentLevelScore;
    private Vector2 _currentSpawnPosition;
    private bool _isHit;
    private bool _isLaunching;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeGame();
    }

    private void InitializeGame()
    {
        _ballSpawner.SpawnBalls();
        _blockSystem.AllBlocksDestroyed += OnAllBlocksDestroyed;
        _currentSpawnPosition = _spawnPos.position;
    }

    private void OnDestroy()
    {
        _blockSystem.AllBlocksDestroyed -= OnAllBlocksDestroyed;
    }

    private void Start()
    {
        StartNewGame();
        _currentLevel = ScoreManager.Instance.LevelScore;
        UIManager.Instance.ShowLevelStart(_currentLevel);
    }

    public void StartNewGame()
    {
        _currentLevel = ScoreManager.Instance.GetLevelScore;
        _levelGenerator.GenerateNextLevel();
        ScoreManager.Instance.ResetCurrentLevelScore();
    }

    public void OnLevelStarted(int levelNumber)
    {
        Debug.Log($"Level {levelNumber} started!");
        // Можно добавить UI-оповещение
    }

    private void OnAllBlocksDestroyed()
    {
        if (_levelGenerator.GetQuitStatus()) return;

        _currentLevelScore = ScoreManager.Instance.GetLevelScore;
        StartCoroutine(LevelCompleteCoroutine());
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        yield return new WaitForSeconds(1f); // Задержка перед показом UI

        // Сохраняем данные и показываем UI
        ScoreManager.Instance.CompleteLevel(_currentLevel);
        UIManager.Instance.ShowLevelComplete(_currentLevelScore, ScoreManager.Instance.CurrentScore);
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        _levelGenerator.GenerateNextLevel();
        BallLauncher.Instance.ResetAllBallsToSpawn(_spawnPos.position);
        _currentSpawnPosition = _spawnPos.position;
        ScoreManager.Instance.ResetLevelScore();
        UIManager.Instance.ShowLevelStart(_currentLevel);
        BallSpawner.Instance.AddBall();
        BallLauncher.Instance.AddBallCount();
        GemManager.Instance.AddGems(10);
    }

    public void OnFirstBallHitBottom(Vector2 hitPosition)
    {
        if (!_isHit)
        {
            _currentSpawnPosition = hitPosition;
            _isHit = true;
        }
        _ballLauncher.RelocateAllBalls(_currentSpawnPosition);
    }

    public IBlockSystem BlockSystem => _blockSystem;
    public void ResetHitStatus() => _isHit = false;
    public void ResetLaunchStatus() => _isLaunching = false;
    public void SetLaunchStatus() => _isLaunching = true;
    public bool LaunchStatus() => _isLaunching;
    public Vector2 GetCurrentSpawnPosition() => _currentSpawnPosition;
    public int GetLevelScore => _currentLevelScore;
}