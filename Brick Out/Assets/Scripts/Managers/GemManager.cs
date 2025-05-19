using System;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance { get; private set; }
    public event Action OnGemsChanged;

    [SerializeField] private GemData _gemData;
    [SerializeField] private int _gemsPerLevel = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadGems();
        }
    }
    public int GetTotalGems() => _gemData.TotalGems;

    public void AddGems(int amount)
    {
        _gemData.TotalGems += amount;
        SaveGems();
        OnGemsChanged?.Invoke();
    }

    public bool TrySpendGems(int amount)
    {
        if (_gemData.TotalGems >= amount)
        {
            _gemData.TotalGems -= amount;
            SaveGems();
            OnGemsChanged?.Invoke();
            return true;
        }
        return false;
    }

    private void LoadGems() => _gemData = JsonUtility.FromJson<GemData>(PlayerPrefs.GetString("GemData")) ?? new GemData();
    private void SaveGems() => PlayerPrefs.SetString("GemData", JsonUtility.ToJson(_gemData));
}