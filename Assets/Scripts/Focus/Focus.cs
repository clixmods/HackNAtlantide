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
    #region Events
    public delegate void EventFocus();
    public delegate void EventSwitchFocus(Transform target);
    public static event EventFocus OnFocusEnable;
    public static event EventFocus OnFocusDisable;
    public static event EventSwitchFocus OnFocusSwitch;
    #endregion
    [Header("Input")]
    [Tooltip("Input to enable and disable focus mode")]
    [SerializeField] private InputButtonScriptableObject inputEnableFocus;
    [Tooltip("Input Vector to switch between the list of target")]
    [SerializeField] private InputVectorScriptableObject inputSwitchTarget;
    
    private List<ITargetable> _targetablesInScene;
    private List<Transform> _targetable = new List<Transform>();
    
    private bool _focusIsEnable;
    [SerializeField] CinemachineVirtualCamera cameraVirtualFocus;
    private FocusCinemachineTargetGroup _cinemachineTargetGroup;
    private int _currentTargetIndex;
    private GameObject _nofocusVirtualCamera;
    private PlayerInstanceScriptableObject _playerInstanceScriptableObject;
    private Transform _previousTarget;
    #region Properties
    public Transform currentTarget
    {
        get
        {
            if (_targetable.Count == 0)
                return null;

            return _targetable[CurrentTargetIndex];
        }
    }
    public int CurrentTargetIndex
    {
        get { return _currentTargetIndex; }
        set
        {
            _currentTargetIndex = value;
            if (_currentTargetIndex < 0)
            {
                _currentTargetIndex = _targetable.Count-1;
            }
            else if(_currentTargetIndex > _targetable.Count - 1)
            {
                _currentTargetIndex = 0;
            }
        }
    }
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        _targetablesInScene = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();
        _playerInstanceScriptableObject = Resources.Load<PlayerInstanceScriptableObject>("PlayerInstance");
        // Input Behaviour
        inputEnableFocus.OnValueChanged += InputEnableFocusOnChanged;
        inputSwitchTarget.OnValueChanged += InputSwitchTargetOnChanged;
        // Check camera transition
        CinemachineCameraVirtualTransition.OnPostCameraChanged += CameraTransitionOnCameraChanged;
        // Setup target group
        _cinemachineTargetGroup = GetComponent<FocusCinemachineTargetGroup>();
    }

    private void Start()
    {
        DisableFocus();
    }

    private void CameraTransitionOnCameraChanged(CinemachineVirtualCamera newCameraVirtual)
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
        var cameraPosition = cameraVirtualFocus.transform.position;
        var cameraRotation = cameraVirtualFocus.transform.rotation;
        float timeTransition = 2;
        while (timeElapsed < timeTransition)
        {
            timeElapsed += Time.deltaTime;
            if(newCameraVirtual.gameObject != _nofocusVirtualCamera)
                yield break;

            var t = timeElapsed / timeTransition;
            cameraVirtualFocus.transform.position = Vector3.Lerp(cameraPosition,newCameraVirtual.transform.position , t);
            cameraVirtualFocus.transform.rotation = Quaternion.Lerp(cameraRotation, newCameraVirtual.transform.rotation, t);
            yield return null;
        }
  
        
    }
    private void InputSwitchTargetOnChanged(Vector2 value)
    {
        if (value.magnitude == 0) return;
        
        if (value.x > 0 || value.y > 0)
        {
            CurrentTargetIndex++;
        }
        else if(value.x < 0 || value.y < 0)
        {
            CurrentTargetIndex--;
        }
        Switch();
    }

    private void Switch()
    {
        if (_previousTarget == currentTarget)
            return;
        
        OnFocusSwitch?.Invoke(currentTarget);
        _cinemachineTargetGroup.SwitchToTarget(currentTarget);
        _previousTarget = currentTarget;
    }

    private void InputEnableFocusOnChanged(bool value)
    {
        if (value)
        {
            _focusIsEnable = !_focusIsEnable;
            if (_focusIsEnable)
            {
                CurrentTargetIndex = 0;
                cameraVirtualFocus.gameObject.SetActive(true); 
                OnFocusEnable?.Invoke();
                Switch();
            }
            else
            {
                DisableFocus();
            }
        }
        
    }

    private void GenerateInteractableObject()
    {
        _targetable.Clear();
        
        foreach (ITargetable target in _targetablesInScene)
        {
            try
            {
                if (target.transform != null && target.CanBeTarget)
                    _targetable.Add(target.transform);
            }
            catch
            {
                Debug.Log("Horrible things happened!");
                _targetablesInScene = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();
                return;
            }

        }
        // Sort the list to have the nearest in first
        _targetable.Sort(delegate(Transform t1, Transform t2){
            return
                Vector3.Distance(
                        t1.position,_playerInstanceScriptableObject.Player.transform.position
                    )
                 
                    .CompareTo(
                        Vector3.Distance(
                            t2.position,
                            _playerInstanceScriptableObject.Player.transform.position
                        )
                    )
                ;
        });
        
    }

    private void DisableFocus()
    {
        cameraVirtualFocus.gameObject.SetActive(false); 
        OnFocusDisable?.Invoke();
        if (_nofocusVirtualCamera != null)
        {
            _nofocusVirtualCamera.SetActive(true);
        }
        _focusIsEnable = false;
    }
    
    private void Update()
    {
        GenerateInteractableObject();
        if (!_focusIsEnable)
        {
            CurrentTargetIndex = 0;
            if (currentTarget != _previousTarget)
            {
                Switch();
            }
        }
    }
}
