using _2DGame.Scripts.Save;
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


public class Settings : MonoBehaviourSaveable
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

    [SerializeField] private Resolution _screenResolution;
    public Resolution ScreenResolution
    {
        get { return _screenResolution; }
        set
        {
            _screenResolution = value;
            Screen.SetResolution(_screenResolution.width,_screenResolution.height,_windowMode==FullScreenMode.ExclusiveFullScreen);
            OnScreenResolutionValueChanged?.Invoke(_screenResolution);
        }
    }
    public Action<Resolution> OnScreenResolutionValueChanged;
    #endregion


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        DataPersistentHandler.Load(this, this.SaveID.ToString());
    }

    private void OnApplicationQuit()
    {
        DataPersistentHandler.Save(this, this.SaveID.ToString());
    }
    
    public class SettingsSaveData : SaveData
    {
        public float volumeMusic;
        public float volumeSFX;
        public float volumeGeneral;
        public RumblerIntensity rumblerIntensity;
        public FullScreenMode windowMode;
    }
    public override void OnLoad(string data)
    {
        SettingsSaveData settingsSaveData = JsonUtility.FromJson<SettingsSaveData>(data);
        _volumeMusic = settingsSaveData.volumeMusic;
        _volumeSFX = settingsSaveData.volumeSFX;
        _volumeGeneral = settingsSaveData.volumeGeneral;
        _rumblerIntensity = settingsSaveData.rumblerIntensity;
        _windowMode = settingsSaveData.windowMode;
    }

    public override void OnSave(out SaveData saveData)
    {
        SettingsSaveData settingsSaveData = new SettingsSaveData();

        settingsSaveData.volumeMusic = _volumeMusic;
        settingsSaveData.volumeSFX = _volumeSFX;
        settingsSaveData.rumblerIntensity = _rumblerIntensity;
        settingsSaveData.volumeGeneral = _volumeGeneral;
        settingsSaveData.windowMode = _windowMode;

        saveData = settingsSaveData;
    }

    public override void OnReset()
    {
        throw new NotImplementedException();
    }
}

