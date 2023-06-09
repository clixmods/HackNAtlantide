﻿using Cinemachine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
    [RequireComponent(typeof(TriggerBox))]
    public class CinemachineCameraVirtualTransition : MonoBehaviour
    {
        public delegate void CameraEvent(CinemachineVirtualCamera newCinemachineVirtualCamera );
        public static event CameraEvent OnCameraChanged;
        public static event CameraEvent OnPostCameraChanged;
        private TriggerBox _zoneVolume;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private bool disableCameraInAwake = true;
        [SerializeField] private bool disableCameraOnExit;
        [SerializeField] private bool followPlayer;

        #region Monobehaviour

        private void Awake()
        {
            if (disableCameraInAwake)
            {
                _cinemachineVirtualCamera.gameObject.SetActive(false);
            }
            OnCameraChanged += DesactiveCamera;
           
        }

        private void OnDestroy()
        {
            OnCameraChanged -= DesactiveCamera;
        }

        private void Start()
        {
            _zoneVolume = GetComponent<TriggerBox>();
            _zoneVolume.EventOnTriggerEnter.AddListener(TriggerEnter);
            if (followPlayer)
            {
                _cinemachineVirtualCamera.Follow = PlayerInstanceScriptableObject.Player.transform;
            }

            if (!disableCameraInAwake)
            {
                ActiveCamera();
            }
        }
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Handles.DrawLine(transform.position, _cinemachineVirtualCamera.transform.position);
        }
#endif

        #endregion

        public void Init(CinemachineVirtualCamera cinemachineVirtualCamera)
        {
            _cinemachineVirtualCamera = cinemachineVirtualCamera;
        }
        public void TriggerEnter()
        {
            if (_cinemachineVirtualCamera.gameObject.activeSelf)
            {
                return;
            }

            ActiveCamera();
        }

        void ActiveCamera()
        {
            OnCameraChanged?.Invoke(_cinemachineVirtualCamera);
            _cinemachineVirtualCamera.gameObject.SetActive(true);
            OnPostCameraChanged?.Invoke(_cinemachineVirtualCamera);
        }
        public void DesactiveCamera(CinemachineVirtualCamera newCinemachineVirtualCamera)
        {
            _cinemachineVirtualCamera.gameObject.SetActive(false);
        }
    }
