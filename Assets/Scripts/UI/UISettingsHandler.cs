using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsHandler : MonoBehaviour
{
    [SerializeField] Slider _generalVolumeSlider;
    [SerializeField] Slider _volumeMusicSlider;
    [SerializeField] Slider _volumeSFXSlider;
    [SerializeField] TMP_Dropdown _rumblerDropDown;
    [SerializeField] TMP_Dropdown _windowModeDropDown;
    // Start is called before the first frame update
    void OnEnable()
    {
        _generalVolumeSlider.onValueChanged.AddListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.AddListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.AddListener(MusicSlider);
        _rumblerDropDown.onValueChanged.AddListener(RumblerDropDown);
        _windowModeDropDown.onValueChanged.AddListener(WindowModeDropDown);
    }
    private void OnDisable()
    {
        _generalVolumeSlider.onValueChanged.RemoveListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.RemoveListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.RemoveListener(MusicSlider);
        _rumblerDropDown.onValueChanged.RemoveListener(RumblerDropDown);
        _windowModeDropDown.onValueChanged.RemoveListener(WindowModeDropDown);
    }

    private void Start()
    {
        LoadSettingsValues();
    }

    void LoadSettingsValues()
    {
        _generalVolumeSlider.value = Settings.Instance.VolumeGeneral;
        _volumeMusicSlider.value = Settings.Instance.VolumeMusic;
        _volumeSFXSlider.value = Settings.Instance.VolumeSFX;
        _rumblerDropDown.value = (int)Settings.Instance.RumblerIntensity;
        _windowModeDropDown.value = (int)Settings.Instance.WindowMode;
    }
    void GeneralSlider(float value)
    {
        Settings.Instance.VolumeGeneral = value;
    }
    void MusicSlider(float value)
    {
        Settings.Instance.VolumeMusic = value;
    }
    void SFXSlider(float value)
    {
        Settings.Instance.VolumeSFX = value;
    }
    void RumblerDropDown(int value)
    {
        Settings.Instance.RumblerIntensity = (RumblerIntensity)value;
    }
    void WindowModeDropDown(int value)
    {
        Settings.Instance.WindowMode = (FullScreenMode)value;
    }
}
