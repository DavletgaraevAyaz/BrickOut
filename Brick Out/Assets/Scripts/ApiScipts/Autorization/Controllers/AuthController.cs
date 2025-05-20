using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthController : MonoBehaviour
{
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Text errorText;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }
    private void ShowError(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(ApiService.Instance.LoginAsync(username, password, (success, message) =>
        {
            if (success)
            {
                SceneManager.LoadScene("StartScene");
            }
            else
            {
                ShowError(message);
            }
        }));

    }
}
