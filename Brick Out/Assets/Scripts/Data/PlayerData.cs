using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int TotalScore;
    public int CurrentLevel;
    public int HighestUnlockedLevel;


    // Конструктор с значениями по умолчанию
    public PlayerData()
    {
        TotalScore = 0;
        CurrentLevel = 1;
    }

    // Метод для сохранения данных
    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    // Метод для загрузки данных
    public static PlayerData LoadData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return new PlayerData(); // Возвращаем новые данные, если сохранений нет
    }

    // Метод для добавления очков
    public void AddScore(int amount)
    {
        TotalScore += amount;
        SaveData(); // Автосохранение при изменении
    }



    // Метод для разблокировки нового уровня
    public void UnlockNextLevel()
    {
        CurrentLevel++;
        SaveData();
    }
}