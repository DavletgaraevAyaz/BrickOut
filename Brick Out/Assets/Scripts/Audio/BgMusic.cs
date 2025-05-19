using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    private static BgMusic _instance;
    private void Awake()
    {
        // Если экземпляр уже существует и это не текущий объект - уничтожаем
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Сохраняем ссылку на первый экземпляр
        _instance = this;

        // Запрещаем уничтожение при загрузке новой сцены
        DontDestroyOnLoad(gameObject);
    }
}

