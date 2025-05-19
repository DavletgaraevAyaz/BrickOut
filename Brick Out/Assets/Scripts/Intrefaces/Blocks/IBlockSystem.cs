using System;

public interface IBlockSystem
{
    void RegisterBlock(IDamageable block);
    void UnregisterBlock(IDamageable block);
    int TotalBlocks { get; }
    event Action AllBlocksDestroyed;
}