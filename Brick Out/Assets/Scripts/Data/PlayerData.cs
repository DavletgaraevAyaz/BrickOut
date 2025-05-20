using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int CurrentLevel;
    public int HighestUnlockedLevel;

    public PlayerData()
    {
        CurrentLevel = 1;
    }

    public void SaveData(string userId)
    {
        PlayerPrefs.SetInt($"CurrentLevel_{userId}", CurrentLevel);
        PlayerPrefs.Save();
    }

    public static PlayerData LoadData(string userId)
    {
        var data = new PlayerData();

        if (PlayerPrefs.HasKey($"CurrentLevel_{userId}"))
        {
            data.CurrentLevel = PlayerPrefs.GetInt($"CurrentLevel_{userId}");
            data.HighestUnlockedLevel = PlayerPrefs.GetInt($"HighestUnlockedLevel_{userId}");
        }

        return data;
    }

    public void UnlockNextLevel(string userId)
    {
        CurrentLevel++;
        SaveData(userId);
    }
}