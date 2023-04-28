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
    
    private List<ITargetable> _itargetablesInScene;
    private List<Transform> _targetable = new List<Transform>();
    
    private bool _focusIsEnable;
    [SerializeField] CinemachineVirtualCamera cameraVirtualFocus;
    private FocusCinemachineTargetGroup _cinemachineTargetGroup;
    private int _currentTargetIndex;
    private GameObject _nofocusVirtualCamera;
    private Transform _previousTarget;
    #region Properties
    public Transform CurrentTarget
    {
        get
        {
            if (_targetable.Count == 0)
                return null;
            
            return _targetable[CurrentTargetIndex];
        }
    }
    private int CurrentTargetIndex
    {
        get { return _currentTargetIndex; }
        set
        {
            if (_targetable.Count == 0)
            {
                _currentTargetIndex = 0;
                return;
            }
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
        _itargetablesInScene = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();
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
    private void Switch()
    {
        if (CurrentTarget != null && _previousTarget == CurrentTarget)
        {
            
            return;
        }

        if (_focusIsEnable && _previousTarget != null)
        {
            _previousTarget.GetComponent<ITargetable>().OnUntarget();
        }
        
        OnFocusSwitch?.Invoke(CurrentTarget);
        _cinemachineTargetGroup.SwitchToTarget(CurrentTarget);
        _previousTarget = CurrentTarget;
        if (_focusIsEnable)
        {
            CurrentTarget.GetComponent<ITargetable>().OnTarget();
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
                if (CurrentTarget != null)
                {
                    CurrentTarget.GetComponent<ITargetable>().OnTarget();
                }
                //Switch();
            }
            else
            {
                DisableFocus();
            }
        }
        
    }

    private bool _forceSwitch = false;
    private void GenerateTargetableList()
    {
        if (_itargetablesInScene.Count == 0)
        {
            return; 
        }
        _targetable = new List<Transform>();
        foreach (ITargetable target in _itargetablesInScene)
        {
            
            try // If the list _itargetablesInScene have no change, we will go in try everytime
            {
                if (target.transform != null && target.CanBeTarget)
                    _targetable.Add(target.transform);
            }
            catch // If the list _itargetablesInScene have a destroyed itargetable, we catch it to fix the null ref.
            {
                Debug.LogWarning("A ITargetable has been destroyed ! Its better to not destroy them in a same scene");
                _itargetablesInScene = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();
                // If the current target has been destroyed, we need to get the nearest target in the next generation
                if (_previousTarget == null)
                {
                    _forceSwitch = true;
                }
                // We need a correct targetableList in this frame.
                GenerateTargetableList();
                return;
            }

        }
        TargetableSortByNearest();
        if (_forceSwitch)
        {
            Switch();
            _forceSwitch = false;
        }
    }

    private void TargetableSortByNearest()
    {
        // Sort the list to have the nearest in first
        _targetable.Sort(delegate(Transform t1, Transform t2)
        {
            return
                Vector3.Distance(
                        t1.position, PlayerInstanceScriptableObject.Player.transform.position
                    )
                    .CompareTo(
                        Vector3.Distance(
                            t2.position,
                            PlayerInstanceScriptableObject.Player.transform.position
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
        if (_previousTarget != null)
        {
            _previousTarget.GetComponent<ITargetable>().OnUntarget();
        }
        
    }
    
    private void Update()
    {
        GenerateTargetableList();
        if (!_focusIsEnable)
        {
            CurrentTargetIndex = 0;
            if (CurrentTarget != _previousTarget)
            {
                Switch();
            }
        }
    }
}
