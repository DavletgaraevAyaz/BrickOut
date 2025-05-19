using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour, IBlockSystem
{
    public static BlockSystem Instance { get; private set; }
    private readonly HashSet<IDamageable> _activeBlocks = new HashSet<IDamageable>();
    public int TotalBlocks => _activeBlocks.Count;
    public event Action AllBlocksDestroyed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterBlock(IDamageable block)
    {
        if (block == null) return;

        _activeBlocks.Add(block);
        block.OnDestroyed += OnBlockDestroyed;

        if (block is DamageableBlock damageableBlock)
        {
            damageableBlock.OnBlockDestroyedWithScore += HandleBlockDestroyedWithScore;
        }
    }

    public void UnregisterBlock(IDamageable block)
    {
        if (block == null || !_activeBlocks.Contains(block)) return;

        _activeBlocks.Remove(block);
        block.OnDestroyed -= OnBlockDestroyed;

        if (_activeBlocks.Count == 0)
            AllBlocksDestroyed?.Invoke();
    }

    private void OnBlockDestroyed(IDamageable block) => UnregisterBlock(block);

    private void HandleBlockDestroyedWithScore(int score)
    {
        ScoreManager.Instance.AddScore(score);
    }

    public IEnumerable<IDamageable> GetAllBlocks() => _activeBlocks;
}