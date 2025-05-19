using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text _ballCountText;
    [SerializeField] private Text _gemCountText;

    private void Start()
    {
        BallSpawner.Instance.OnBallCountChanged += UpdateBallCount;
        GemManager.Instance.OnGemsChanged += UpdateGemDisplay;
        UpdateGemDisplay();

        // Инициализация начального значения
        UpdateBallCount(BallLauncher.Instance.GetBallCount());
    }

    public void UpdateBallCount(int count) => _ballCountText.text = count.ToString();
    public void UpdateGemDisplay() => _gemCountText.text = GemManager.Instance.GetTotalGems().ToString();

    private void OnDestroy()
    {
        BallSpawner.Instance.OnBallCountChanged -= UpdateBallCount;
        GemManager.Instance.OnGemsChanged -= UpdateGemDisplay;
    }
}
