using GameCells.Utilities;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private CanvasGroup _settingsCanvasGroup;
    [SerializeField] private Button _backButton;

    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Header("Gameplay")]
    [SerializeField] private Slider _sensitivitySlider;


    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SfxVolume = "SfxVolume";
    private const string Sensitivity = "Sensitivity";

    private const float VolumeMultiplier = 20;

    public event Action OnShow;
    public event Action OnHide;
    public event Action OnSensitivityChanged;

    private void Start()
    {
        if (PlayerPrefs.HasKey(MasterVolume))
        {
            float volume = PlayerPrefs.GetFloat(MasterVolume);
            SetMasterVolume(volume);
        }

        if (PlayerPrefs.HasKey(MusicVolume))
        {
            float volume = PlayerPrefs.GetFloat(MusicVolume);
            SetMusicVolume(volume);
        }

        if (PlayerPrefs.HasKey(SfxVolume))
        {
            float volume = PlayerPrefs.GetFloat(SfxVolume);
            SetSfxVolume(volume);
        }
    }

    public void ShowSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(_backButton.gameObject);

        _settingsCanvasGroup.alpha = 1;
        _settingsCanvasGroup.interactable = true;
        _settingsCanvasGroup.blocksRaycasts = true;

        OnShow?.Invoke();
    }

    public void HideSettingsMenu()
    {
        _settingsCanvasGroup.alpha = 0;
        _settingsCanvasGroup.interactable = false;
        _settingsCanvasGroup.blocksRaycasts = false;

        OnHide?.Invoke();
    }

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MasterVolume, volume);

        _masterVolumeSlider.value = volume;

        volume = volume / 10; //Convert to 0-1

        if (volume == 0)
            volume = 0.0001f;

        _audioMixer.SetFloat(MasterVolume, Mathf.Log10(volume) * VolumeMultiplier);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MusicVolume, volume);

        _musicVolumeSlider.value = volume;

        volume = volume / 10; //Convert to 0-1

        if (volume == 0)
            volume = 0.0001f;

        _audioMixer.SetFloat(MusicVolume, Mathf.Log10(volume) * VolumeMultiplier);
    }

    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat(SfxVolume, volume);

        _sfxVolumeSlider.value = volume;

        volume = volume / 10; //Convert to 0-1

        if (volume == 0)
            volume = 0.0001f;

        _audioMixer.SetFloat(SfxVolume, Mathf.Log10(volume) * VolumeMultiplier);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(Sensitivity, _sensitivitySlider.value);
        _sensitivitySlider.value = sensitivity;

        OnSensitivityChanged?.Invoke();
    }

    public float GetSensitivity()
    {
        if (PlayerPrefs.HasKey(Sensitivity))
        {
            return PlayerPrefs.GetFloat(Sensitivity);
        }
        else
        {
            return -1;
        }
    }

    public void ResetToDefault()
    {
        SetMasterVolume(10);
        SetMusicVolume(10);
        SetSfxVolume(10);
        SetSensitivity(5);
    }
}
