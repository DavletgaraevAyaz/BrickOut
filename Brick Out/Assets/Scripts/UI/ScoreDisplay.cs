using System.Collections;
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

    private void Awake()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScore;
        ScoreManager.Instance.OnLevelScoreChanged += UpdateLevelScore;
        StartCoroutine(SetScores());
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScore;
            ScoreManager.Instance.OnLevelScoreChanged -= UpdateLevelScore;
        }
    }

    private void Update()
    {
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

    private IEnumerator SetScores()
    {
        yield return new WaitForSeconds(0.1f);
        _displayedScore = ScoreManager.Instance.TotalScore;
        Debug.Log($"Dis score:{_displayedScore}");
        _levelScore = GameManager.Instance.GetLevelScore;
        _targetScore = _displayedScore;
        UpdateText();
    }

}