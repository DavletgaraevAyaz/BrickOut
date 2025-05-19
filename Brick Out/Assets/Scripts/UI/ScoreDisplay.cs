using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _scoreLevelText;
    [SerializeField] private string _prefix = "Total score: ";
    [SerializeField] private string _prefixLevel = "Score: ";
    [SerializeField] private float _scoreChangeSpeed = 1f;

    private int _displayedScore;
    private int _levelScore;
    private int _targetScore;

    private void Start()
    {
        // Подписываемся на событие изменения очков
        ScoreManager.Instance.OnScoreChanged += UpdateScore;
        ScoreManager.Instance.OnLevelScoreChanged += UpdateLevelScore;
        _displayedScore = ScoreManager.Instance.CurrentScore;
        _levelScore = GameManager.Instance.GetLevelScore;
        _targetScore = _displayedScore;
        UpdateText();
    }

    private void OnDisable()
    {
        // Отписываемся при выключении
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScore;
            ScoreManager.Instance.OnLevelScoreChanged -= UpdateLevelScore;
        }
    }

    private void Update()
    {
        // Плавное изменение числа
        if (_displayedScore != _targetScore)
        {
            _displayedScore = (int)Mathf.Lerp(_displayedScore, _targetScore,_scoreChangeSpeed * Time.deltaTime);

            if (Mathf.Abs(_displayedScore - _targetScore) < 10)
            {
                _displayedScore = _targetScore;
            }

            UpdateText();
        }
    }

    private void UpdateScore(int newScore)
    {
        _targetScore = newScore;
    }
    private void UpdateLevelScore(int newScore)
    {
        _levelScore = newScore;
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = _prefix + _displayedScore.ToString("N0");
        _scoreLevelText.text = _prefixLevel + _levelScore.ToString("N0");
    }

    // Для анимации при получении очков
    public void PlayScoreAnimation()
    {
        // Можно добавить DOTween анимацию или другие эффекты
        _scoreText.transform.localScale = Vector3.one * 1.2f;
        Invoke(nameof(ResetAnimation), 0.3f);
    }

    private void ResetAnimation()
    {
        _scoreText.transform.localScale = Vector3.one;
    }
}