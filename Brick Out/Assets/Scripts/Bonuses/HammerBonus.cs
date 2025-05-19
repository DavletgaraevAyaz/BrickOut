using UnityEngine;

public class HammerBonus : MonoBehaviour, IBonus
{
    [SerializeField] private int _cost = 10;

    public void Activate()
    {
        if (!GemManager.Instance.TrySpendGems(_cost)) return;

        var lowestBlock = FindLowestBlock();
        if (lowestBlock != null)
            lowestBlock.TakeDamage(lowestBlock.CurrentHealth);
    }

    private IDamageable FindLowestBlock()
    {
        IDamageable lowestBlock = null;
        float minY = float.MaxValue;

        foreach (var block in BlockSystem.Instance.GetAllBlocks())
        {
            MonoBehaviour blockMono = block as MonoBehaviour;
            if (blockMono.transform.position.y < minY)
            {
                minY = blockMono.transform.position.y;
                lowestBlock = block;
            }
        }
        return lowestBlock;
    }
}