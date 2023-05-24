using System;
using UnityEditor;
using UnityEngine;


    [ExecuteAlways]
    [RequireComponent(typeof(UnityEngine.Light))]
    public class LightLOD : MonoBehaviour
    {
        private Light m_light;
        [Header("Attenuation")]
        [Range(0,2)]
        [SerializeField] private float attenuationInner = 0;
        [Range(0,2)]
        [SerializeField] private float attenuationOuter = 1;
        [Header("LOD")]
        [SerializeField] private float maxDistanceVisible = 10;

        [SerializeField] private bool HideWhenOutOfCameraVision;
        

        private void OnEnable()
        {
            m_light = GetComponent<Light>();
        }

        private void OnDisable()
        {
           
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                float attenuationDistance =  1 - (Vector3.Distance(Camera.main.transform.position, transform.position) / maxDistanceVisible) ;
               // volumetricBoost = _cachedVolumetricBoost * attenuationDistance;
                if (HideWhenOutOfCameraVision && transform.position.IsOutOfCameraVision())
                {
                    m_light.enabled = false;
                }
                if (Vector3.Distance(Camera.main.transform.position, transform.position) > maxDistanceVisible )
                {
                    m_light.enabled = false;
                }
                else
                {
                    m_light.enabled = true;
                }
            }
           
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, Vector3.Distance(Camera.main.transform.position, transform.position).ToString());
        }
#endif
    }
