using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallLauncher : MonoBehaviour, IBallLauncher
{
    public static BallLauncher Instance { get; private set; }
    [SerializeField] private float _delayBetweenBalls = 0.1f;
    [SerializeField] private  List<Ball> _inactiveBalls = new List<Ball>();
    [SerializeField] private bool _isLaunching;
    [SerializeField] private List<Ball> _inactiveBallInLaunchTime = new List<Ball>();

    private Vector2 _currentBallsPos;
    private int _ballCount;

    public int GetBallCount() => _inactiveBalls.Count + _inactiveBallInLaunchTime.Count;
    public int AddBallCount() => _ballCount++;

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        _ballCount = _inactiveBalls.Count;
    }


    private void Update()
    {
        if(_inactiveBalls.Count == _ballCount)
        {
            GameManager.Instance.ResetLaunchStatus();
        }

        if (!_isLaunching && _inactiveBallInLaunchTime.Count > 0)
        {
            var ballsToAdd = new List<Ball>(_inactiveBallInLaunchTime);
            _inactiveBallInLaunchTime.Clear();

            foreach (Ball ball in ballsToAdd)
            {
                if (!_inactiveBalls.Contains(ball))
                {
                    _inactiveBalls.Add(ball);
                }
            }
        }
    }
    public void AddBall(Ball ball)
    {
        if (!_inactiveBalls.Contains(ball) && !_isLaunching)
        {
            _inactiveBalls.Add(ball);
        }
        else
        {
            _inactiveBallInLaunchTime.Add(ball);
        }
    }

    public void LaunchBalls(Vector2 direction, float force)
    {
        if (_isLaunching || _inactiveBalls.Count == 0) return;
        StartCoroutine(LaunchBallsCoroutine(direction, force));
    }

    private IEnumerator LaunchBallsCoroutine(Vector2 direction, float force)
    {
        _isLaunching = true;

        while (_inactiveBalls.Count > 0)
        {
            var ball = _inactiveBalls[0];
            _inactiveBalls.RemoveAt(0);
            ball.Initialize(force);
            ball.Launch(direction);
            yield return new WaitForSeconds(_delayBetweenBalls);
        }
        GameManager.Instance.ResetHitStatus();
        _isLaunching = false;
    }

    public void ResetAllBallsToSpawn(Vector2 spawnPos)
    {
        // Остановить все запущенные корутины
        StopAllCoroutines();
        _isLaunching = false;

        // Найти все мячи на сцене
        Ball[] allBalls = FindObjectsOfType<Ball>();

        foreach (Ball ball in allBalls)
        {
            // Деактивировать физику
            ball.DisablePhysics();

            // Вернуть на стартовую позицию
            ball.transform.position = spawnPos;

            // Добавить в список неактивных, если еще не добавлен
            if (!_inactiveBalls.Contains(ball))
            {
                _inactiveBalls.Add(ball);
            }
        }

        // Очистить временный список
        _inactiveBallInLaunchTime.Clear();
    }

    public void ResetAllBallsToSpawn()
    {
        if (_currentBallsPos == new Vector2()) return;
        // Остановить все запущенные корутины
        StopAllCoroutines();
        _isLaunching = false;

        // Найти все мячи на сцене
        Ball[] allBalls = FindObjectsOfType<Ball>();

        foreach (Ball ball in allBalls)
        {
            // Деактивировать физику
            ball.DisablePhysics();

            // Вернуть на стартовую позицию
            ball.transform.position = _currentBallsPos;

            // Добавить в список неактивных, если еще не добавлен
            if (!_inactiveBalls.Contains(ball))
            {
                _inactiveBalls.Add(ball);
            }
        }

        // Очистить временный список
        _inactiveBallInLaunchTime.Clear();
    }

    public void RelocateAllBalls(Vector2 newPosition)
    {
        _inactiveBalls.ForEach(ball => ball.transform.position = newPosition);
        _currentBallsPos = newPosition;
    }
}