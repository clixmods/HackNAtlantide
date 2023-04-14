using System;
using Cinemachine;
using UnityEngine;

    [RequireComponent(typeof(ZoneVolume))]
    public class CinemachineCameraVirtualTransition : MonoBehaviour
    {
        private ZoneVolume _zoneVolume;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private void Start()
        {
            _zoneVolume = GetComponent<ZoneVolume>();
            _cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _zoneVolume.EventOnTriggerEnter.AddListener(ActiveCamera);
            _zoneVolume.EventOnTriggerEnd.AddListener(DesactiveCamera);
            _zoneVolume.LayersMaskWithInteract = LayerMask.GetMask("Player");
        }

        public void ActiveCamera()
        {
            _cinemachineVirtualCamera.gameObject.SetActive(true);
        }
        public void DesactiveCamera()
        {
            _cinemachineVirtualCamera.gameObject.SetActive(false);
        }
    }
