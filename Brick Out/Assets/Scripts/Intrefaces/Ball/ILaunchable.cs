using UnityEngine;

public interface ILaunchable
{
    void Launch(Vector2 direction);
    void DisablePhysics();
    void EnablePhysics();
}