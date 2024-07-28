using GameCells.UI;
using GameCells.Utilities;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private GCUI_Panel _settingsUIPanel;
    //[SerializeField] private CanvasGroup _settingsCanvasGroup;
    [SerializeField] private Button _backButton;

    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Header("Gameplay")]
    [SerializeField] private Slider _mouseSensitivitySlider;
    [SerializeField] private Slider _controllerSensitivitySlider;


    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SfxVolume = "SfxVolume";
    private const string MouseSensitivity = "MouseSensitivity";
    private const string ControllerSensitivity = "ControllerSensitivity";

    private const float VolumeMultiplier = 20;

    public event Action OnShow;
    public event Action OnHide;
    public event Action OnMouseSensitivityChanged;
    public event Action OnControllerSensitivityChanged;

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
        //EventSystem.current.SetSelectedGameObject(_backButton.gameObject);

        _settingsUIPanel.Show();
        //_settingsCanvasGroup.alpha = 1;
        //_settingsCanvasGroup.interactable = true;
        //_settingsCanvasGroup.blocksRaycasts = true;

        OnShow?.Invoke();
    }

    public void HideSettingsMenu()
    {
        _settingsUIPanel.Hide();
        //_settingsCanvasGroup.alpha = 0;
        //_settingsCanvasGroup.interactable = false;
        //_settingsCanvasGroup.blocksRaycasts = false;

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

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(MouseSensitivity, _mouseSensitivitySlider.value);
        _mouseSensitivitySlider.value = sensitivity;

        OnMouseSensitivityChanged?.Invoke();
    }

    public float GetMouseSensitivity()
    {
        if (PlayerPrefs.HasKey(MouseSensitivity))
        {
            return PlayerPrefs.GetFloat(MouseSensitivity);
        }
        else
        {
            return -1;
        }
    }

    public void SetControllerSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(ControllerSensitivity, _controllerSensitivitySlider.value);
        _controllerSensitivitySlider.value = sensitivity;

        OnControllerSensitivityChanged?.Invoke();
    }

    public float GetControllerSensitivity()
    {
        if (PlayerPrefs.HasKey(ControllerSensitivity))
        {
            return PlayerPrefs.GetFloat(ControllerSensitivity);
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
        SetMouseSensitivity(5);
    }
}
