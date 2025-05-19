using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool _isPause = false;
    public void IsPause()
    {
        if( !_isPause)
        {
            _isPause = true;
            Time.timeScale = 0f;
        }
        else
        {
            _isPause = false;
            Time.timeScale = 1f;
        }
    }
}
