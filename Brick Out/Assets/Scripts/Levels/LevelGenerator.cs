using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private int _initialBlocksCount = 10;
    [SerializeField] private int _blocksIncrementPerLevel = 2;
    [SerializeField] private Vector2 _spawnArea = new Vector2(8f, 4f);
    [SerializeField] private Vector2 _spawnOffset = new Vector2(0, 3f);
    [SerializeField] private float _minDistanceBetweenBlocks = 0.8f;

    [Header("Difficulty Settings")]
    [SerializeField] private BlockType[] _blockTypes;
    [SerializeField] private int _maxHealthIncreaseInterval = 5;

    [Header("References")]
    [SerializeField] private DamageableBlock _blockPrefab;
    [SerializeField] private Transform _blocksContainer;

    [Header("Chaotic Spawn Settings")]
    [SerializeField] private float _positionNoise = 0.3f; // Шум для позиций
    [SerializeField] private int _spawnAttemptsMultiplier = 50;

    private int _currentLevel = 1;
    private List<DamageableBlock> _activeBlocks = new List<DamageableBlock>();
    private List<Vector2> _usedPositions = new List<Vector2>();
    private bool _isQuitting;

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }
    private void Start()
    {
        _currentLevel = ScoreManager.Instance.LevelScore;
    }

    public void GenerateNextLevel()
    {
        if (_isQuitting) return;
        ClearCurrentLevel();

        int blocksCount = _initialBlocksCount + (_currentLevel - 1) * _blocksIncrementPerLevel;
        GenerateBlocks(blocksCount);

        _currentLevel++;
        GameManager.Instance.OnLevelStarted(_currentLevel);
    }

    private void GenerateBlocks(int count)
    {
        _usedPositions.Clear();
        int blocksToCreate = count;

        while (blocksToCreate > 0)
        {
            if (TrySpawnBlock())
                blocksToCreate--;
        }
    }

    private bool TrySpawnBlock()
    {
        int maxAttempts = _spawnAttemptsMultiplier * _activeBlocks.Count + 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector2 position = GetChaoticPosition();

            if (!IsPositionTooClose(position))
            {
                CreateBlock(position);
                _usedPositions.Add(position);
                return true;
            }
            attempts++;
        }
        return false;
    }
    private Vector2 GetChaoticPosition()
    {
        // Основная область спавна
        float x = Random.Range(-_spawnArea.x / 2, _spawnArea.x / 2);
        float y = Random.Range(_spawnOffset.y, _spawnOffset.y + _spawnArea.y);

        // Добавляем шум к позиции
        Vector2 noise = new Vector2(
            Random.Range(-_positionNoise, _positionNoise),
            Random.Range(-_positionNoise, _positionNoise)
        );

        return new Vector2(x, y) + noise;
    }

    private bool IsPositionTooClose(Vector2 position)
    {
        foreach (var usedPos in _usedPositions)
        {
            float distance = Vector2.Distance(position, usedPos);
            if (distance < _minDistanceBetweenBlocks)
                return true;

            // Дополнительная проверка для краев
            if (distance < _minDistanceBetweenBlocks * 0.7f)
                return true;
        }
        return false;
    }

    private void CreateBlock(Vector2 position)
    {
        DamageableBlock block = Instantiate(_blockPrefab, position, Quaternion.identity, _blocksContainer);
        ConfigureBlock(block);
        _activeBlocks.Add(block);
    }

    private void ConfigureBlock(DamageableBlock block)
    {
        BlockType blockType = GetBlockTypeForLevel();
        int health = Random.Range(blockType.minHealth, blockType.maxHealth + 1);
        int score = 100 * _currentLevel * health * blockType.scoreMultiplier;

        //block.SetHealth(health);
        block.SetScoreValue(score);
        block.SetColor(GetHealthColor(blockType.baseColor, health));
    }

    private BlockType GetBlockTypeForLevel()
    {
        int levelAdjusted = Mathf.Max(1, _currentLevel / _maxHealthIncreaseInterval);
        int typeIndex = Mathf.Min(levelAdjusted, _blockTypes.Length - 1);
        return _blockTypes[typeIndex];
    }

    private Color GetHealthColor(Color baseColor, int health)
    {
        float healthFactor = (float)health / _blockTypes[_blockTypes.Length - 1].maxHealth;
        return Color.Lerp(baseColor, Color.white, healthFactor * 0.5f);
    }

    private void ClearCurrentLevel()
    {
        foreach (var block in _activeBlocks)
        {
            if (block != null)
            {
                // Уничтожаем только блоки, а не контейнер
                Destroy(block.gameObject);
            }
        }
        _activeBlocks.Clear();
    }

    public int GetActiveBlocksCount() => _activeBlocks.Count;
    public bool GetQuitStatus() => _isQuitting;
}