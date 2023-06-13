using _2DGame.Scripts.Save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Settings;

[CreateAssetMenu(fileName = "SettingsData")]
public class SettingsScriptableObject : ScriptableObjectSaveable
{
    [SerializeField] AudioMixer _audioMixer;

    #region Audio
    [SerializeField] private float _volumeMusic;
    private string VolumeMusicID = "VolumeMusic";
    public float VolumeMusic
    {
        get { return _volumeMusic; }
        set
        {
            _volumeMusic = Mathf.Clamp(value, -80, 0);
            if (_audioMixer.GetFloat(VolumeMusicID, out float volume))
            {
                _audioMixer.SetFloat(VolumeMusicID, _volumeMusic);
            }
            OnVolumeMusicChanged?.Invoke(_volumeMusic);
        }
    }
    public Action<float> OnVolumeMusicChanged;

    [SerializeField] private float _volumeSFX;

    private string VolumeSFXID = "VolumeSFX";
    public float VolumeSFX
    {
        get { return _volumeSFX; }
        set
        {
            _volumeSFX = Mathf.Clamp(value, -80, 0);
            if (_audioMixer.GetFloat(VolumeSFXID, out float volume))
            {
                _audioMixer.SetFloat(VolumeSFXID, _volumeSFX);
            }
            OnVolumeSFXChanged?.Invoke(_volumeSFX);
        }
    }
    public Action<float> OnVolumeSFXChanged;

    private string VolumeGeneralID = "VolumeMaster";
    [SerializeField] private float _volumeGeneral;
    public float VolumeGeneral
    {
        get { return _volumeGeneral; }
        set
        {
            _volumeGeneral = Mathf.Clamp(value, -80, 0);
            if (_audioMixer.GetFloat(VolumeGeneralID, out float volume))
            {
                _audioMixer.SetFloat(VolumeGeneralID, _volumeGeneral);
            }
            OnVolumeGeneralChanged?.Invoke(_volumeGeneral);
        }
    }
    public Action<float> OnVolumeGeneralChanged;
    #endregion

    #region Rumbler
    [SerializeField] private RumblerIntensity _rumblerIntensity;
    public RumblerIntensity RumblerIntensity
    {
        get { return _rumblerIntensity; }
        set
        {
            _rumblerIntensity = value;
            OnRumblerIntensityChanged?.Invoke(_rumblerIntensity);
        }
    }
    public Action<RumblerIntensity> OnRumblerIntensityChanged;
    #endregion

    #region Graphics

    //WindowMode
    [SerializeField] private FullScreenMode _windowMode;
    public FullScreenMode WindowMode
    {
        get { return _windowMode; }
        set
        {
            _windowMode = value;
            OnWindowModeValueChanged?.Invoke(_windowMode);
            Screen.fullScreenMode = _windowMode;
        }
    }
    public Action<FullScreenMode> OnWindowModeValueChanged;


    //Resolution
    [SerializeField] private Resolution _screenResolution;
    public Resolution ScreenResolution
    {
        get { return _screenResolution; }
        set
        {
            _screenResolution = value;
            Screen.SetResolution(_screenResolution.width, _screenResolution.height, _windowMode == FullScreenMode.ExclusiveFullScreen);
            OnScreenResolutionValueChanged?.Invoke(_screenResolution);
        }
    }

    public Action<Resolution> OnScreenResolutionValueChanged;

    //Camera Shake
    [SerializeField] private bool _useCameraShake = true;
    public bool UseCameraShake
    {
        get { return _useCameraShake; }
        set { _useCameraShake = value; }
    }


    //RefreshRate
    [SerializeField] private int _maxRefreshRate;
    public int MaxRefreshRate
    {
        get { return _maxRefreshRate; }
        set
        {
            _maxRefreshRate = value;
            if(_lockFPS)
            {
                Application.targetFrameRate = _maxRefreshRate;
            }
            else
            {
                Application.targetFrameRate = -1;
            }
        }
    }

    [SerializeField] private bool _lockFPS;
    public bool LockFps
    {
        get { return _lockFPS; }
        set
        {
            _lockFPS = value;
            if (_lockFPS)
            {
                Application.targetFrameRate = _maxRefreshRate;
            }
            else
            {
                Application.targetFrameRate = -1;
            }
        }
    }
    [SerializeField] private bool _showFPS;
    public bool ShowFps
    {
        get { return _showFPS; }
        set
        {
            _showFPS = value;
        }
    }

    //Vsync
    [SerializeField] private bool _useVsync;
    public bool UseVSYnc
    {
        get { return _useVsync; }
        set
        {
            _useVsync = value;
            if (_useVsync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
    }
    #endregion
    public override void OnLoad(string data)
    {
        SettingsSaveData settingsSaveData = JsonUtility.FromJson<SettingsSaveData>(data);
        VolumeMusic = settingsSaveData.volumeMusic;
        VolumeSFX = settingsSaveData.volumeSFX;
        VolumeGeneral = settingsSaveData.volumeGeneral;
        RumblerIntensity = settingsSaveData.rumblerIntensity;
        WindowMode = settingsSaveData.windowMode;
        UseCameraShake = settingsSaveData.useCameraShake;
        MaxRefreshRate = settingsSaveData.maxRefreshRate;
        ScreenResolution = settingsSaveData.screenResolution;
        LockFps = settingsSaveData.lockFps;
        UseVSYnc = settingsSaveData.useVsync;
        ShowFps = settingsSaveData.showFps;
    }

    public override void OnReset()
    {
        /*_volumeMusic = 1;
        _volumeSFX = 1;
        _volumeGeneral = 1;
        _rumblerIntensity = RumblerIntensity.mid;
        _windowMode = FullScreenMode.ExclusiveFullScreen;
        _useCameraShake = true;
        _maxRefreshRate = 144;*/
        _screenResolution = Screen.currentResolution;
        /*_lockFPS = true;
        _useVsync = true;
        _showFPS = false;*/
    }

    public override void OnSave(out SaveData saveData)
    {
        SettingsSaveData settingsSaveData = new SettingsSaveData();

        settingsSaveData.volumeMusic = _volumeMusic;
        settingsSaveData.volumeSFX = _volumeSFX;
        settingsSaveData.rumblerIntensity = _rumblerIntensity;
        settingsSaveData.volumeGeneral = _volumeGeneral;
        settingsSaveData.windowMode = _windowMode;
        settingsSaveData.useCameraShake = _useCameraShake;
        settingsSaveData.maxRefreshRate = _maxRefreshRate;
        settingsSaveData.screenResolution = _screenResolution;
        settingsSaveData.lockFps = _lockFPS;
        settingsSaveData.useVsync = _useVsync;
        settingsSaveData.showFps = _showFPS;
        saveData = settingsSaveData;
    }

}

public class SettingsSaveData : SaveData
{
    public float volumeMusic;
    public float volumeSFX;
    public float volumeGeneral;
    public RumblerIntensity rumblerIntensity;
    public FullScreenMode windowMode;
    public bool useCameraShake;
    public Resolution screenResolution;
    public int maxRefreshRate;
    public bool lockFps;
    public bool useVsync;
    public bool showFps;
}
public enum RumblerIntensity
{
    none,
    low,
    mid,
    high
}
