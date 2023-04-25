using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Focus : MonoBehaviour
{
    [FormerlySerializedAs("objectsTargettable")] public List<Transform> objectsTargetable;
    [SerializeField] private float minimumDistanceToBeTargettable;
    [SerializeField] private InputButtonScriptableObject inputEnableFocus;
    [SerializeField] private InputVectorScriptableObject inputSwitchTarget;
    private bool _focusIsEnable;
    [SerializeField] CinemachineVirtualCamera cameraVirtual;
    private CinemachineTargetGroup _cinemachineTargetGroup;

    private int _indexTargettable;
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
        var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>();
        objectsTargetable.Clear();
        foreach (ITargetable target in targets)
        {
            objectsTargetable.Add(target.transform);
        }
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        inputEnableFocus.OnValueChanged += InputEnableFocusOnOnValueChanged;
        inputSwitchTarget.OnValueChanged += InputSwitchTargetOnOnValueChanged;
        CinemachineCameraVirtualTransition.OnCameraChanged += CinemachineCameraVirtualTransitionOnOnCameraChanged;
    }

    private void CinemachineCameraVirtualTransitionOnOnCameraChanged(CinemachineVirtualCamera newCameraVirtual)
    {
        cameraVirtual.transform.position = newCameraVirtual.transform.position;
        cameraVirtual.transform.rotation = newCameraVirtual.transform.rotation;
    }

    private void InputSwitchTargetOnOnValueChanged(Vector2 value)
    {
        if (value.x > 0 || value.y > 0)
        {
            IndexTargettable++;
        }
        else if(value.x < 0 || value.y < 0)
        {
            IndexTargettable--;
        }
        _cinemachineTargetGroup.m_Targets[1].target = objectsTargetable[IndexTargettable].transform;
    }

    private void InputEnableFocusOnOnValueChanged(bool value)
    {
        if (value)
        {
            _focusIsEnable = !_focusIsEnable;
            cameraVirtual.gameObject.SetActive(_focusIsEnable); 
            if (_focusIsEnable)
            {
                _cinemachineTargetGroup.m_Targets[1].target = objectsTargetable[IndexTargettable].transform;
                cameraVirtual.Follow = GameObject.FindWithTag("Player").transform;
            }
        }
        
    }
    
}
