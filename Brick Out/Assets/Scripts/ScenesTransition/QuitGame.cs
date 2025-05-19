using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitTheGame()
    {
#if UNITY_EDITOR
        // В редакторе - выходим из режима Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В билде - закрываем приложение
        Application.Quit();
#endif
    }
}

