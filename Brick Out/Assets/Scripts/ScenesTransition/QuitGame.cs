using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitTheGame()
    {
#if UNITY_EDITOR
        // � ��������� - ������� �� ������ Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // � ����� - ��������� ����������
        Application.Quit();
#endif
    }
}

