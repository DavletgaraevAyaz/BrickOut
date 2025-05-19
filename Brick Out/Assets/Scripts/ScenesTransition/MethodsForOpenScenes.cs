using UnityEngine;
using UnityEngine.SceneManagement;

public class MethodsForOpenScenes : MonoBehaviour
{
    [SerializeField] private string _scenePath;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(_scenePath))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_scenePath);
        }
    }
}
