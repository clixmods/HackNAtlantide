using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Focus : MonoBehaviour
{
    public delegate void EventFocus();
    public delegate void EventSwitchFocus(Transform target);
    public static event EventFocus OnFocusEnable;
    public static event EventFocus OnFocusDisable;
    public static event EventSwitchFocus OnFocusSwitch;
    
    [FormerlySerializedAs("objectsTargettable")] public List<Transform> objectsTargetable;
    [SerializeField] private float minimumDistanceToBeTargettable;
    [SerializeField] private InputButtonScriptableObject inputEnableFocus;
    [SerializeField] private InputVectorScriptableObject inputSwitchTarget;
    private bool _focusIsEnable;
    [SerializeField] CinemachineVirtualCamera cameraVirtual;
    private CinemachineTargetGroup _cinemachineTargetGroup;
    private CinemachineBrain _cinemachineBrain;
    private int _indexTargettable;
    [SerializeField] private float switchTargetMultiplier = 1f;
    private bool _isSwitching;
    private GameObject _nofocusVirtualCamera;

    public int IndexTargettable
    {
        get { return _indexTargettable; }
        set
        {
            _indexTargettable = value;
            if (_indexTargettable < 0)
            {
                _indexTargettable = objectsTargetable.Count-1;
            }
            else if(_indexTargettable > objectsTargetable.Count - 1)
            {
                _indexTargettable = 0;
            }
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        
        var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>();
        objectsTargetable.Clear();
        foreach (ITargetable target in targets)
        {
            objectsTargetable.Add(target.transform);
        }
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        inputEnableFocus.OnValueChanged += InputEnableFocusOnOnValueChanged;
        inputSwitchTarget.OnValueChanged += InputSwitchTargetOnOnValueChanged;
        CinemachineCameraVirtualTransition.OnPostCameraChanged += CinemachineCameraVirtualTransitionOnOnCameraChanged;
        _cinemachineTargetGroup.m_Targets = new CinemachineTargetGroup.Target[3];
        _cinemachineTargetGroup.m_Targets[0].weight = 1;
        _cinemachineTargetGroup.m_Targets[0].target = GameObject.FindWithTag("Player").transform;
        _cinemachineTargetGroup.m_Targets[1].weight = 1;
        _cinemachineTargetGroup.m_Targets[2].weight = 1;
    }
    
    private void CinemachineCameraVirtualTransitionOnOnCameraChanged(CinemachineVirtualCamera newCameraVirtual)
    {
        if (_focusIsEnable)
        {
            _nofocusVirtualCamera = newCameraVirtual.gameObject;
            _nofocusVirtualCamera.SetActive(false);
        }
        StartCoroutine(LerpCameraPosition(newCameraVirtual));
    }
    
    IEnumerator LerpCameraPosition(CinemachineVirtualCamera newCameraVirtual)
    {
        float timeElapsed = 0;
        var cameraPosition = cameraVirtual.transform.position;
        var cameraRotation = cameraVirtual.transform.rotation;
        float timeTransition = 2;
        while (timeElapsed < timeTransition )
        {
            timeElapsed += Time.deltaTime;
            if(newCameraVirtual.gameObject != _nofocusVirtualCamera)
                yield break;

            var t = timeElapsed / timeTransition;
            cameraVirtual.transform.position = Vector3.Lerp(cameraPosition,newCameraVirtual.transform.position , t);
            cameraVirtual.transform.rotation = Quaternion.Lerp(cameraRotation,  newCameraVirtual.transform.rotation, t);
            
            yield return null;
        }
  
        
    }

    private void InputSwitchTargetOnOnValueChanged(Vector2 value)
    {
        if (value.magnitude == 0) return;
        
        if (value.x > 0 || value.y > 0)
        {
            IndexTargettable++;
        }
        else if(value.x < 0 || value.y < 0)
        {
            IndexTargettable--;
        }
        
        StopCoroutine(ChangeTarget());
        StartCoroutine(ChangeTarget());
    }

    private void InputEnableFocusOnOnValueChanged(bool value)
    {
        if (value)
        {
            _focusIsEnable = !_focusIsEnable;
           
            if (_focusIsEnable)
            {
                cameraVirtual.gameObject.SetActive(true); 
                OnFocusEnable?.Invoke();
                StartCoroutine(ChangeTarget());
            }
            else
            {
                DisableFocus();
            }
        }
        
    }

    private void DisableFocus()
    {
        cameraVirtual.gameObject.SetActive(false); 
        OnFocusDisable?.Invoke();
        if (_nofocusVirtualCamera != null)
            _nofocusVirtualCamera.SetActive(true);
        _focusIsEnable = false;
    }

    IEnumerator ChangeTarget()
        {
            if(_isSwitching)
                yield break;
            
            _cinemachineTargetGroup.m_Targets[2].target = objectsTargetable[IndexTargettable].transform;
            OnFocusSwitch?.Invoke(_cinemachineTargetGroup.m_Targets[2].target);
            _cinemachineTargetGroup.m_Targets[2].weight = 0;
            _cinemachineTargetGroup.m_Targets[1].weight = 1;
            while ( _cinemachineTargetGroup.m_Targets[1].weight > 0 && _cinemachineTargetGroup.m_Targets[2].target != null)
            {
                _isSwitching = true;
                var t = Time.deltaTime * switchTargetMultiplier;
                _cinemachineTargetGroup.m_Targets[1].weight = Mathf.Clamp(_cinemachineTargetGroup.m_Targets[1].weight - t,0,1);
                _cinemachineTargetGroup.m_Targets[2].weight = Mathf.Clamp(_cinemachineTargetGroup.m_Targets[2].weight + t,0,1);
                yield return null;
            }
            _isSwitching = false;
            _cinemachineTargetGroup.m_Targets[1].target = _cinemachineTargetGroup.m_Targets[2].target;
            _cinemachineTargetGroup.m_Targets[1].weight = 1;
            _cinemachineTargetGroup.m_Targets[2].target = null;
            
        }

    private void Update()
    {
        if (_cinemachineTargetGroup.m_Targets[1].target != null &&
            Vector3.Distance(_cinemachineTargetGroup.m_Targets[0].target.position, _cinemachineTargetGroup.m_Targets[1].target.position) >
            minimumDistanceToBeTargettable)
        {
            DisableFocus();
        }
    }
}
