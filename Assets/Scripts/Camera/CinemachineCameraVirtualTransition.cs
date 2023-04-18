using System;
using Cinemachine;
using UnityEngine;

    [RequireComponent(typeof(ZoneVolume))]
    public class CinemachineCameraVirtualTransition : MonoBehaviour
    {
        public delegate void CameraEvent();

        public event CameraEvent OnCameraChanged;
        private ZoneVolume _zoneVolume;
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
            _zoneVolume = GetComponent<ZoneVolume>();
            _zoneVolume.EventOnTriggerEnter.AddListener(ActiveCamera);
           // _zoneVolume.EventOnTriggerEnd.AddListener(DesactiveCamera);
            _zoneVolume.LayersMaskWithInteract = LayerMask.GetMask("Player");
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
