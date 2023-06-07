using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsHandler : MonoBehaviour
{
    [SerializeField] Slider _generalVolumeSlider;
    [SerializeField] Slider _volumeMusicSlider;
    [SerializeField] Slider _volumeSFXSlider;
    [SerializeField] TMP_Dropdown _rumblerDropDown;
    [SerializeField] TMP_Dropdown _windowModeDropDown;
    [SerializeField] TMP_Dropdown _screenResolutionDropDown;
    [SerializeField] Toggle _cameraShakeToggle;
    [SerializeField] Toggle _useVsyncToggle;
    [SerializeField] Toggle _lockFpsToggle;
    [SerializeField] Toggle _showFpsToggle;
    [SerializeField] Slider _maxFpsSlider;
    [SerializeField] GameObject _maxFpsParent;
    [SerializeField] TextMeshProUGUI _maxFpsText;

    [SerializeField] SettingsScriptableObject _settingsData;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(WaitForEndOfFrameToLoad());
    }
    private void OnDisable()
    {
        _generalVolumeSlider.onValueChanged.RemoveListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.RemoveListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.RemoveListener(MusicSlider);
        _rumblerDropDown.onValueChanged.RemoveListener(RumblerDropDown);
        _windowModeDropDown.onValueChanged.RemoveListener(WindowModeDropDown);
        _cameraShakeToggle.onValueChanged.RemoveListener(UseCameraShake);
        _screenResolutionDropDown.onValueChanged.RemoveListener(ScreenResolutionDropDown);
        _useVsyncToggle.onValueChanged.RemoveListener(UseVsync);
        _lockFpsToggle.onValueChanged.RemoveListener(LockFps);
        _showFpsToggle.onValueChanged.RemoveListener(ShowFps);
        _maxFpsSlider.onValueChanged.RemoveListener(MaxFps);
        Save();
    }
    IEnumerator WaitForEndOfFrameToLoad()
    {
        yield return new WaitForEndOfFrame();

        LoadSettingsValues();

        _generalVolumeSlider.onValueChanged.AddListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.AddListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.AddListener(MusicSlider);
        _rumblerDropDown.onValueChanged.AddListener(RumblerDropDown);
        _windowModeDropDown.onValueChanged.AddListener(WindowModeDropDown);
        _cameraShakeToggle.onValueChanged.AddListener(UseCameraShake);
        _screenResolutionDropDown.onValueChanged.AddListener(ScreenResolutionDropDown);
        _useVsyncToggle.onValueChanged.AddListener(UseVsync);
        _lockFpsToggle.onValueChanged.AddListener(LockFps);
        _showFpsToggle.onValueChanged.AddListener(ShowFps);
        _maxFpsSlider.onValueChanged.AddListener(MaxFps);
    }

    private void Start()
    {
        LoadSettingsValues();
    }

    void LoadSettingsValues()
    {
        Load();
        _generalVolumeSlider.value = _settingsData.VolumeGeneral;
        _volumeMusicSlider.value = _settingsData.VolumeMusic;
        _volumeSFXSlider.value = _settingsData.VolumeSFX;
        _rumblerDropDown.value = (int)_settingsData.RumblerIntensity;
        _windowModeDropDown.value = (int)_settingsData.WindowMode;
        _cameraShakeToggle.isOn = _settingsData.UseCameraShake;

        switch(_settingsData.ScreenResolution.width)
        {
            case 720:

                _screenResolutionDropDown.value =0;
                break;
            case 1280:
                _screenResolutionDropDown.value = 1;
                break;
            case 1920:
                _screenResolutionDropDown.value = 2;
                break;
            case 3840:
                _screenResolutionDropDown.value = 3;
                break;
                default:
                _screenResolutionDropDown.value = 2;
                break;
        }

        _useVsyncToggle.isOn = _settingsData.UseVSYnc;
        _lockFpsToggle.isOn = _settingsData.LockFps;
        _showFpsToggle.isOn = _settingsData.ShowFps;
        _maxFpsSlider.value = _settingsData.MaxRefreshRate;
        _maxFpsParent.SetActive(_settingsData.LockFps);
    }
    void GeneralSlider(float value)
    {
        _settingsData.VolumeGeneral = value;
    }
    void MusicSlider(float value)
    {
        _settingsData.VolumeMusic = value;
    }
    void SFXSlider(float value)
    {
        _settingsData.VolumeSFX = value;
    }
    void RumblerDropDown(int value)
    {
        _settingsData.RumblerIntensity = (RumblerIntensity)value;
    }
    void WindowModeDropDown(int value)
    {
        _settingsData.WindowMode = (FullScreenMode)value;
    }
    void ScreenResolutionDropDown(int value)
    {
        Resolution resolution = new();
        switch(value)
        {
            case 0:
                resolution.width = 720;
                resolution.height = 480;
            break;
            case 1:
                resolution.width = 1280;
                resolution.height = 720;
                break;
            case 2:
                resolution.width = 1920;
                resolution.height = 1080;
                break;
            case 3:
                resolution.width = 3840;
                resolution.height = 2160;
                break;
        }
        _settingsData.ScreenResolution = resolution;
    }
    public void UseVsync(bool value)
    {
        _settingsData.UseVSYnc = value;
    }
    public void LockFps(bool value)
    {
        _settingsData.LockFps = value;
        _maxFpsParent.SetActive(_settingsData.LockFps);
    }
    public void ShowFps(bool value)
    {
        _settingsData.ShowFps = value;
    }
    public void MaxFps(float value)
    {
        _settingsData.MaxRefreshRate = Mathf.RoundToInt(value);
        _maxFpsText.text = Mathf.RoundToInt(value).ToString();
    }
    public void UseCameraShake(bool value)
    {
        _settingsData.UseCameraShake = value;
    }

    public void Save()
    {
        DataPersistentHandler.Save(_settingsData, _settingsData.name);
    }
    public void Load()
    {
        DataPersistentHandler.Load(_settingsData, _settingsData.name);
    }
}
