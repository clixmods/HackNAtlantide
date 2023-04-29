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
    // Start is called before the first frame update
    void OnEnable()
    {
        _generalVolumeSlider.onValueChanged.AddListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.AddListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.AddListener(MusicSlider);
        _rumblerDropDown.onValueChanged.AddListener(RumblerDropDown);
    }
    private void OnDisable()
    {
        _generalVolumeSlider.onValueChanged.RemoveListener(GeneralSlider);
        _volumeSFXSlider.onValueChanged.RemoveListener(SFXSlider);
        _volumeMusicSlider.onValueChanged.RemoveListener(MusicSlider);
        _rumblerDropDown.onValueChanged.RemoveListener(RumblerDropDown);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
