using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    public void QuitMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
