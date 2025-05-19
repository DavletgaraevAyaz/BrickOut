using System;

public interface IDamageable
{
    void TakeDamage(int amount);
    int CurrentHealth { get; }
    int MaxHealth { get; }
    event Action<IDamageable> OnDestroyed;
}