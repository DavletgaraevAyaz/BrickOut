using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner Instance { get; private set; }
    public event Action<int> OnBallCountChanged;
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _ballCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void UpdateBallCount()
    {
        OnBallCountChanged?.Invoke(_ballLauncher.GetBallCount());
    }

    public void SpawnBalls()
    {
        if (PlayerPrefs.HasKey("Balls"))
        {
            _ballCount = PlayerPrefs.GetInt("Balls");
        }
        for (int i = 0; i < _ballCount; i++)
        {
            var ball = Instantiate(_ballPrefab, _spawnPoint.position, Quaternion.identity);
            _ballLauncher.AddBall(ball.GetComponent<Ball>());
        }
        UpdateBallCount();
    }

    public void AddBall()
    {
        var ball = Instantiate(_ballPrefab, _spawnPoint.position, Quaternion.identity);
        _ballLauncher.AddBall(ball.GetComponent<Ball>());
        UpdateBallCount();
        _ballCount++;

        PlayerPrefs.SetInt("Balls", _ballCount);
    }

    public void AddBalls()
    {
        for(int i = 0; i<10; i++)
        {
            var ball = Instantiate(_ballPrefab, _spawnPoint.position, Quaternion.identity);
            _ballLauncher.AddBall(ball.GetComponent<Ball>());
            UpdateBallCount();
            _ballCount++;
        }

        PlayerPrefs.SetInt("Balls", _ballCount);
    }

    public void ReturnToPool(Ball ball)
    {
        _ballLauncher.AddBall(ball);
        UpdateBallCount();
    }

    public IEnumerator AddInactiveBalls(Ball ball)
    {
        yield return new WaitForSeconds(2f);
        _ballLauncher.AddBall(ball);
    }
}