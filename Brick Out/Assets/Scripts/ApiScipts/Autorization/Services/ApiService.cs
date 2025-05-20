using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class ApiService : MonoBehaviour,IAuthService
{
    public static ApiService Instance { get; private set; }
    public string UserId() => GetUserIdFromToken().ToString();

    private string _baseUrl = "http://localhost:5179/api/";
    private string _authToken;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoginAsync(string username, string password, Action<bool, string> callback)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(_baseUrl + "Auth/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                _authToken = response.token;
                callback(true, "Login successful");
                SetAuthToken(response.token);
            }
            else
            {
                callback(false, request.error);
            }
        }
    }
    public IEnumerator GetUserScore(Action<int, string> callback)
    {
        var userId = GetUserIdFromToken();

        string url = $"{_baseUrl}Score/{userId}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            if (!string.IsNullOrEmpty(_authToken))
            {
                request.SetRequestHeader("Authorization", $"Bearer {_authToken}");
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Raw server response: {request.downloadHandler.text}");

                try
                {
                    Debug.Log(request.downloadHandler.text + "123");
                    var jsonResponse = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);
                    Debug.Log(jsonResponse.score);
                    callback(jsonResponse.score, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing coins response: {ex.Message}");
                    callback(0, "Îøèáêà îáðàáîòêè äàííûõ.");
                }
            }
            else
            {
                Debug.LogError($"Request error: {request.error}");
                callback(0, request.error);
            }
        }
    }

    public IEnumerator AddCoins(int score, Action<int, string> callback)
    {
        var userId = GetUserIdFromToken();
        var coinsData = new AddScoreRequest { amount = score};
        string jsonData = JsonUtility.ToJson(coinsData);

        string url = $"{_baseUrl}Score/add/{userId}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);
                    callback(response.score, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing response: {ex.Message}");
                    callback(0, "Error parsing server response");
                }
            }
            else
            {
                Debug.LogError($"Request error: {request.error}");
                callback(0, request.error);
            }
        }
    }

    private int? GetUserIdFromToken()
    {
        if (string.IsNullOrEmpty(_authToken))
            return null;

        try
        {
            var tokenParts = _authToken.Split('.');
            if (tokenParts.Length < 2)
                return null;

            var payload = DecodeBase64(tokenParts[1]);
            var payloadJson = JsonUtility.FromJson<JwtPayload>(payload);

            if (int.TryParse(payloadJson.nameid, out int userId))
            {
                return userId;
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing token: {ex.Message}");
            return null;
        }
    }

    private string DecodeBase64(string base64)
    {
        try
        {
            int padding = base64.Length % 4;
            if (padding != 0)
            {
                base64 += new string('=', 4 - padding);
            }

            byte[] data = Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error decoding Base64: {ex.Message}");
            throw;
        }
    }


    public void SetAuthToken(string token)
    {
        _authToken = token;
    }

    public string GetToken() => _authToken;
}
