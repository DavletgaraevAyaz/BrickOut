using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Collider2D))]
public class DamageableBlock : MonoBehaviour, IDamageable
{
    [Header("Block Settings")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color[] _healthColors;
    [SerializeField] private int _scoreValue = 100;
    [SerializeField] private TMP_Text _healthText;

    [Header("VFX Settings")]
    [SerializeField] private GameObject _destructionClip;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => _maxHealth;
    public event Action<IDamageable> OnDestroyed;
    public event Action<int> OnBlockDestroyedWithScore;

    private IBlockSystem _blockSystem;
    private GameObject _audioSource;
    private AudioSource _audioSourcePlayer;
    private float sparkLifeTime = 0.5f;


    private void Start()
    {
        SetupBlock();

        _audioSource = GameObject.FindGameObjectWithTag("Audio");
        _audioSourcePlayer = _audioSource.GetComponent<AudioSource>();
    }

    private void SetupBlock()
    {
        _maxHealth = UnityEngine.Random.Range(1, 100);
        CurrentHealth = _maxHealth;
        _blockSystem = GameManager.Instance.BlockSystem;
        _blockSystem.RegisterBlock(this);
        UpdateVisual();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            DestroyBlock();
            GameObject spark = Instantiate(_destructionClip, transform.position, Quaternion.identity);

            Destroy(spark, sparkLifeTime);
        }
        else
        {
            UpdateVisual();
        }
    }


    private void UpdateColor()
    {
        if (_healthColors.Length == 0) return;

        // Получаем номер десятка (0-9)
        int decadeIndex = Mathf.FloorToInt((CurrentHealth - 1) / 10f);

        // Ограничиваем индекс массива цветов
        decadeIndex = Mathf.Clamp(decadeIndex, 0, _healthColors.Length - 1);

        // Если цветов меньше 10, используем последний доступный цвет для старших десятков
        if (decadeIndex >= _healthColors.Length)
        {
            decadeIndex = _healthColors.Length - 1;
        }

        _renderer.color = _healthColors[decadeIndex];
    }

    private void UpdateVisual()
    {
        UpdateHealthText();
        UpdateColor();
    }

    private void DestroyBlock()
    {
        _audioSourcePlayer.Play();
        OnBlockDestroyedWithScore?.Invoke(_scoreValue);
        _blockSystem.UnregisterBlock(this);
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_blockSystem != null)
        {
            _blockSystem.UnregisterBlock(this);
        }
    }

    public void SetScoreValue(int value)
    {
        _scoreValue = value;
    }

    public void SetColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.color = color;
        }
    }

    private void UpdateHealthText()
    {
        _healthText.text = CurrentHealth.ToString();
    }
}