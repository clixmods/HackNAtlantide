using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSUI : MonoBehaviour
{
        [SerializeField] TextMeshProUGUI fpsText;
        float deltaTime;
        [SerializeField] SettingsScriptableObject _settingsData;
        
        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = _settingsData.ShowFps? Mathf.Ceil(fps).ToString() + " fps" : "";
        }
}
