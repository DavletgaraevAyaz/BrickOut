using UnityEngine;
using UnityEngine.UI;

public class BallCounterUI : MonoBehaviour, IBallCountObserver
{
    [SerializeField] private Text _ballCountText;

    public void UpdateBallCount(int count)
    {
        _ballCountText.text = $"Balls: {count}";
    }
}