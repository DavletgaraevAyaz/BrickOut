using UnityEngine;

public interface IBallLauncher
{
    void LaunchBalls(Vector2 direction, float force);
    void AddBall(Ball ball);
}