using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InputZone : MonoBehaviour
{
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }

    public bool IsPositionInZone(Vector2 worldPosition)
    {
        return _collider.OverlapPoint(worldPosition);
    }
}