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
            //_cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
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
           // _zoneVolume.EventOnTriggerEnd.AddListener(DesactiveCamera);
            
            if (followPlayer)
            {
                _cinemachineVirtualCamera.Follow = GameObject.FindWithTag("Player").transform;
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
