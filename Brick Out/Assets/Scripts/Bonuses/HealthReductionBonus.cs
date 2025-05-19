using System.Collections.Generic;
using UnityEngine;

public class HealthReductionBonus : MonoBehaviour, IBonus
{
    [SerializeField] private int _cost = 30;
    [SerializeField] private int _healthToReduce = 20;

    public void Activate()
    {
        if (!GemManager.Instance.TrySpendGems(_cost)) return;

        ReduceAllBlocksHealth();
    }

    private void ReduceAllBlocksHealth()
    {
        if (BlockSystem.Instance == null) return;

        var blocks = BlockSystem.Instance.GetAllBlocks();

        // ������� ����� ������ ��� ���������� ��������
        foreach (var block in new List<IDamageable>(blocks))
        {
            if (block != null)
            {
                // �������� ����� ��������� �����
                block.TakeDamage(_healthToReduce);
            }
        }
    }
}