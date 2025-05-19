using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour, ILaunchable, IPoolable
{
    [SerializeField] private float _force;
    private Rigidbody2D _rb;
    private bool _isLaunched;
    private AudioSource _audioSource;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    public void Initialize(float force)
    {
        _force = force;
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (_isLaunched)
        {
            _rb.velocity = _rb.velocity.normalized * _force;
        }
    }

    public void Launch(Vector2 direction)
    {
        EnablePhysics();
        _rb.velocity = direction * _force;
        _isLaunched = true;
    }

    public void EnablePhysics() => _rb.isKinematic = false;
    public void DisablePhysics()
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        _isLaunched = false;
    }

    public void ReturnToPool() => BallSpawner.Instance.ReturnToPool(this);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BottomWall"))
        {
            DisablePhysics();
            ReturnToPool();
            GameManager.Instance.OnFirstBallHitBottom(transform.position);
        }
        else if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(1);
            _audioSource.Play();
        }
    }
}