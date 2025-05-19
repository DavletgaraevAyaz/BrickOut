using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    private static BgMusic _instance;
    private void Awake()
    {
        // ���� ��������� ��� ���������� � ��� �� ������� ������ - ����������
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ��������� ������ �� ������ ���������
        _instance = this;

        // ��������� ����������� ��� �������� ����� �����
        DontDestroyOnLoad(gameObject);
    }
}

