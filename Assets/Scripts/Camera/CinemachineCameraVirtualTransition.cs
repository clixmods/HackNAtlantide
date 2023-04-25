using System;
using Cinemachine;
using UnityEngine;

    [RequireComponent(typeof(BoxTrigger))]
    public class CinemachineCameraVirtualTransition : MonoBehaviour
    {
        public delegate void CameraEvent();
        public event CameraEvent OnCameraChanged;
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

        private void Start()
        {
            _zoneVolume = GetComponent<BoxTrigger>();
            _zoneVolume.EventOnTriggerEnter.AddListener(ActiveCamera);
            if (followPlayer)
            {
                _cinemachineVirtualCamera.Follow = Resources.Load<PlayerInstanceScriptableObject>("PlayerInstance").Player.transform;
            }
        }

        public void ActiveCamera()
        {
            OnCameraChanged?.Invoke();
            _cinemachineVirtualCamera.gameObject.SetActive(true);
        }
        public void DesactiveCamera()
        {
            _cinemachineVirtualCamera.gameObject.SetActive(false);
        }
    }
