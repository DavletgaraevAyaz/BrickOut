using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int TotalScore;
    public int CurrentLevel;
    public int HighestUnlockedLevel;


    // ����������� � ���������� �� ���������
    public PlayerData()
    {
        TotalScore = 0;
        CurrentLevel = 1;
    }

    // ����� ��� ���������� ������
    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    // ����� ��� �������� ������
    public static PlayerData LoadData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return new PlayerData(); // ���������� ����� ������, ���� ���������� ���
    }

    // ����� ��� ���������� �����
    public void AddScore(int amount)
    {
        TotalScore += amount;
        SaveData(); // �������������� ��� ���������
    }



    // ����� ��� ������������� ������ ������
    public void UnlockNextLevel()
    {
        CurrentLevel++;
        SaveData();
    }
}