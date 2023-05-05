using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

    [RequireComponent(typeof(BoxTrigger))]
    public class CinemachineCameraVirtualTransition : MonoBehaviour
    {
        public delegate void CameraEvent(CinemachineVirtualCamera newCinemachineVirtualCamera );
        public static event CameraEvent OnCameraChanged;
        public static event CameraEvent OnPostCameraChanged;
        private BoxTrigger _zoneVolume;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private bool disableCameraInAwake = true;
        [SerializeField] private bool disableCameraOnExit;
        [SerializeField] private bool followPlayer;
        public void Init(CinemachineVirtualCamera cinemachineVirtualCamera)
        {
            _cinemachineVirtualCamera = cinemachineVirtualCamera;
           
        }
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
            _zoneVolume = GetComponent<BoxTrigger>();
            _zoneVolume.EventOnTriggerEnter.AddListener(ActiveCamera);
            if (followPlayer)
            {
                _cinemachineVirtualCamera.Follow = PlayerInstanceScriptableObject.Player.transform;
            }

            if (!disableCameraInAwake)
            {
                ActiveCamera();
            }
        }

        public void ActiveCamera()
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
