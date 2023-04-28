using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RumblerIntensity
{
    none,
    low,
    mid,
    high
}
public enum WindowMode
{
    Windowed,
    FullScreen,
    FullScreenWindowed
}

[CreateAssetMenu(fileName ="SettingsData")] 
public class Settings : MonoBehaviour
{
    public static Settings Instance;

    #region Audio
    [SerializeField] private float _volumeMusic;
    public float VolumeMusic 
    { 
        get { return _volumeMusic; } 
        set { _volumeMusic = Mathf.Clamp(value,0,1); 
            OnVolumeMusicChanged?.Invoke(_volumeMusic); } 
    }
    public Action<float> OnVolumeMusicChanged;

    [SerializeField] private float _volumeSFX;
    public float VolumeSFX 
    { 
        get { return _volumeSFX; } 
        set { _volumeSFX = Mathf.Clamp(value, 0, 1); 
            OnVolumeSFXChanged?.Invoke(_volumeSFX); } 
    }
    public Action<float> OnVolumeSFXChanged;

    [SerializeField] private float _volumeGeneral;
    public float VolumeGeneral 
    { 
        get { return _volumeGeneral; } 
        set { _volumeGeneral = Mathf.Clamp(value, 0, 1);
            OnVolumeGeneralChanged?.Invoke(_volumeGeneral);
        } 
    }
    public Action<float> OnVolumeGeneralChanged;
    #endregion

    #region Rumbler
    [SerializeField] private RumblerIntensity _rumblerIntensity;
    public RumblerIntensity RumblerIntensity { 
        get { return _rumblerIntensity; } 
        set { _rumblerIntensity = value; 
            OnRumblerIntensityChanged?.Invoke(_rumblerIntensity); 
        } 
    }
    public Action<RumblerIntensity> OnRumblerIntensityChanged;
    #endregion

    #region Graphics
    [SerializeField] private WindowMode _windowMode;
    public WindowMode WindowMode
    {
        get { return _windowMode; }
        set
        {
            _windowMode = value;
            OnWindowModeValueChanged?.Invoke(_windowMode);
        }
    }
    public Action<WindowMode> OnWindowModeValueChanged;

    [SerializeField] private Resolution _screenResolution;
    public Resolution ScreenResolution
    {
        get { return _screenResolution; }
        set
        {
            _screenResolution = value;
            Screen.SetResolution(_screenResolution.width,_screenResolution.height,_windowMode==WindowMode.FullScreen);
            OnScreenResolutionValueChanged?.Invoke(_screenResolution);
        }
    }
    public Action<Resolution> OnScreenResolutionValueChanged;
    #endregion


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

}
