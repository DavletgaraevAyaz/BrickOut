using UnityEngine;

public class ExtraBallsBonus : MonoBehaviour, IBonus
{
    [SerializeField] private int _cost = 20; // Стоимость в гемах

    public void Activate()
    {
        if (!GemManager.Instance.TrySpendGems(_cost)) return;

        if (BallSpawner.Instance != null)
        {
            BallSpawner.Instance.AddBalls();
        }
        else
        {
            Debug.LogError("BallSpawner instance not found!");
        }
    }
}