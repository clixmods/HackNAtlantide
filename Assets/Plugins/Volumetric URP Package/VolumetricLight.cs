using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Post_Process_Custom
{
    [ExecuteAlways]
    [RequireComponent(typeof(Light))]
    public class VolumetricLight : MonoBehaviour
    {
        public static Dictionary<Light, VolumetricLight> AdditionnalVolumetricLights = new Dictionary<Light, VolumetricLight>();
        public static VolumetricLight MainVolumetricLight;
        private Light m_light;
        [SerializeField] private float volumetricBoost = 1;
        [SerializeField] private Color tintColor = Color.white;
        [SerializeField] private bool volumetricCookie = true;
        [SerializeField] private bool volumetricUseLightIntensity = true;
        [Header("Noise")]
        [SerializeField] private Texture blueNoise;
        [SerializeField] private float blueNoiseOffset;
        [SerializeField] private float blueNoiseScale;
        [Header("Attenuation")]
        [SerializeField] private float attenuationInner = 0;
        [SerializeField] private float attenuationOuter = 0;
    /*
     * ref option qhttps://www.youtube.com/watch?v=VpJ94GbWnL8
     * 
     */
        public float VolumetricBoost => volumetricBoost;
        public Color TintColor => tintColor;
        public Texture BlueNoise => blueNoise;
        public float BlueNoiseOffset => blueNoiseOffset;
        public float BlueNoiseScale => blueNoiseScale;
        public Texture CookieTexture => m_light.cookie;
        public float AttenuationInner => attenuationInner;
        public float AttenuationOuter => attenuationOuter;
        private void OnEnable()
        {
            Resources.Load("VolumetricFogShader");
            m_light = GetComponent<Light>();
            if (m_light.type != LightType.Directional)
            {
                AdditionnalVolumetricLights[m_light] = this;
            }
            else
            {
                MainVolumetricLight = this;
            }
        }

        private void OnDisable()
        {
            if (m_light.type != LightType.Directional)
            {
                AdditionnalVolumetricLights.Remove(m_light);
            }
            else
            {
                MainVolumetricLight = null;
            }
        }
        
    }
}