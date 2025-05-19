using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text _buttonText;

    private AudioSource _musicSource;
    private const string _musicOnText = "ON MUSIC";
    private const string _musicOffText = "OFF MUSIC";

    private bool _isMusicOn = true;

    private void Start()
    {
        _musicSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        // Загружаем сохраненные настройки звука
        _isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        UpdateMusicState();
        UpdateButtonText();
    }

    public void ToggleMusic()
    {
        _isMusicOn = !_isMusicOn;
        UpdateMusicState();
        UpdateButtonText();
        SaveSettings();
    }

    private void UpdateMusicState()
    {
        if (_musicSource != null)
        {
            _musicSource.mute = !_isMusicOn;
        }
        else
        {
            Debug.LogWarning("Music source not assigned!");
        }
    }

    private void UpdateButtonText()
    {
        if (_buttonText != null)
        {
            _buttonText.text = _isMusicOn ? _musicOffText : _musicOnText;
        }
        else
        {
            Debug.LogWarning("Button text reference not assigned!");
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicEnabled", _isMusicOn ? 1 : 0);
    }
}