using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Focus : MonoBehaviour
{
    #region Events
    public delegate void EventFocus();
    public delegate void EventSwitchFocus(ITargetable target);
    public static event EventFocus OnFocusEnable;
    public static event EventFocus OnFocusDisable;
    public static event EventSwitchFocus OnFocusSwitch;
    public static event EventFocus OnFocusNoTarget;
    #endregion
    [Header("Input")]
    [Tooltip("Input to enable and disable focus mode")]
    [SerializeField] private InputButtonScriptableObject inputEnableFocus;
    [Tooltip("Input Vector to switch between the list of target")]
    [SerializeField] private InputVectorScriptableObject inputSwitchTarget;
    
    //private List<Targetable> _itargetablesInScene;
    private List<ITargetable> _targetableAvailable = new List<ITargetable>();
    
    public static bool FocusIsEnable;
    [SerializeField] CinemachineVirtualCamera cameraVirtualFocus;
    private FocusCinemachineTargetGroup _cinemachineTargetGroup;
    private int _currentTargetIndex;
    private GameObject _nofocusVirtualCamera;
    private ITargetable _lastcachedTarget;
    #region Properties
    public ITargetable CurrentTarget
    {
        get
        {
            if (_targetableAvailable.Count == 0)
                return null;
          
            
            return _targetableAvailable[CurrentTargetIndex];
        }
    }
    
    private int CurrentTargetIndex
    {
        get
        {
            if (_currentTargetIndex > _targetableAvailable.Count - 1)
            {
                _currentTargetIndex = _targetableAvailable.Count - 1;
            }
            return _currentTargetIndex;
        }
        set
        {
            if (_targetableAvailable.Count == 0)
            {
                _currentTargetIndex = 0;
                return;
            }
            _currentTargetIndex = value;
            if (_currentTargetIndex < 0)
            {
                _currentTargetIndex = _targetableAvailable.Count-1;
            }
            else if(_currentTargetIndex > _targetableAvailable.Count - 1)
            {
                _currentTargetIndex = 0;
            }
        }
    }
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        FocusIsEnable = false;
        //_itargetablesInScene = FindObjectsOfType<Targetable>().ToList();
        // Input Behaviour
        inputEnableFocus.OnValueChanged += InputEnableFocusOnChanged;
        inputSwitchTarget.OnValueChanged += InputSwitchTargetOnChanged;
        // Check camera transition
        CinemachineCameraVirtualTransition.OnPostCameraChanged += CameraTransitionOnCameraChanged;
        // Setup target group
        _cinemachineTargetGroup = GetComponent<FocusCinemachineTargetGroup>();
    }

    private void OnDestroy()
    {
        inputEnableFocus.OnValueChanged -= InputEnableFocusOnChanged;
        inputSwitchTarget.OnValueChanged -= InputSwitchTargetOnChanged;
        CinemachineCameraVirtualTransition.OnPostCameraChanged -= CameraTransitionOnCameraChanged;
    }

    private void Start()
    {
        DisableFocus();
    }

    private void CameraTransitionOnCameraChanged(CinemachineVirtualCamera newCameraVirtual)
    {
        // If its the same camera, not necesary to continue
        if (newCameraVirtual.gameObject == _nofocusVirtualCamera)
            return;
        
        if (FocusIsEnable)
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
        if (_targetableAvailable.Count == 0)
        {
            OnFocusNoTarget?.Invoke();
            DisableFocus();
            return;
        }
        if (CurrentTarget != null && _lastcachedTarget == CurrentTarget)
        {
            OnFocusSwitch?.Invoke(CurrentTarget);
            return;
        }

        if (FocusIsEnable )
        {
            try
            {
                _lastcachedTarget.OnUntarget();
            }
            catch
            {
                Debug.LogWarning("A ITargetable has been destroyed ! Its better to not destroy them in a same scene");
                _lastcachedTarget = null;
            }
        }
    
        
        
        OnFocusSwitch?.Invoke(CurrentTarget);
        if (CurrentTarget != null)
        {
            _cinemachineTargetGroup.SwitchToTarget(CurrentTarget.targetableTransform);
            _lastcachedTarget = CurrentTarget;
            if (FocusIsEnable)
            {
                CurrentTarget.OnTarget();
            }
      
        }
    }

    private void InputSwitchTargetOnChanged(Vector2 value)
    {
        if (value.magnitude <= 0.7) return;
        
        if(InputManager.IsGamepad())
        {
            CurrentTargetIndex = ClosestDotIndex(value);
        }
        else
        {
            if (value.x > 0 || value.y > 0)
            {
                CurrentTargetIndex++;
            }
            else if (value.x < 0 || value.y < 0)
            {
                CurrentTargetIndex--;
            }
        }
        
        Switch();
    }
    int ClosestDotIndex(Vector2 inputValue)
    {
        Camera cam = CameraUtility.Camera;

        Vector3 camForwardOnPlane = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 camRightOnPlane = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;

        Vector3 inputDirection = inputValue.x * camRightOnPlane + inputValue.y * camForwardOnPlane;

        Vector3 playerPos = PlayerInstanceScriptableObject.Player.transform.position;
        Debug.Log(inputValue);
        int index = 0;
        float closestdot = -2;
        for(int i = 0; i < _targetableAvailable.Count; i++)
        {
            float dot = Vector3.Dot(inputDirection,(_targetableAvailable[i].transform.position - playerPos).normalized);

            if(dot > closestdot)
            {
                index = i;
                closestdot = dot;
            }
        }
        return index;
    }
    
    private void InputEnableFocusOnChanged(bool value)
    {
        if (value)
        {
            FocusIsEnable = !FocusIsEnable;
            if (FocusIsEnable)
            {
                CurrentTargetIndex = 0;
                cameraVirtualFocus.gameObject.SetActive(true); 
                OnFocusEnable?.Invoke();
                Switch();
                try
                {
                    if(CurrentTarget != null)
                        CurrentTarget.OnTarget();
                }
                catch
                {
                    Debug.LogWarning("A ITargetable has been destroyed ! Its better to not destroy them in a same scene");
                }
                
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
        if (ITargetable.Targetables.Count == 0)
        {
            if (_targetableAvailable.Count > 0)
            {
                _targetableAvailable = new List<ITargetable>();
            }
            return; 
        }
        _targetableAvailable = new List<ITargetable>();
        foreach (ITargetable targetable in ITargetable.Targetables)
        {
            if (targetable.CanBeTarget)
            {
                _targetableAvailable.Add(targetable);
            }
           
        }


        if (!_targetableAvailable.Contains(_lastcachedTarget))
        {
            _forceSwitch = true;
        }
        
        TargetableSortByNearest();
        if (_forceSwitch)
        {
            Switch();
            _forceSwitch = false;
        }
        
        if (!FocusIsEnable && _targetableAvailable.Count > 0)
        {
            Switch();
        }
    }

    private void TargetableSortByNearest()
    {
        // Sort the list to have the nearest in first
        _targetableAvailable.Sort(delegate(ITargetable t1, ITargetable t2)
        {
            return
                Vector3.Distance(
                        t1.transform.position, PlayerInstanceScriptableObject.Player.transform.position
                    )
                    .CompareTo(
                        Vector3.Distance(
                            t2.transform.position,
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
        FocusIsEnable = false;
        try
        {
            //if (_targetableAvailable.Contains(_lastcachedTarget))
            {
                _lastcachedTarget.OnUntarget();
                _lastcachedTarget = null;
            }
        }
        catch
        {
            _lastcachedTarget = null;
           // Debug.LogWarning("A ITargetable has been destroyed ! Its better to not destroy them in a same scene");
        }
       

        CurrentTargetIndex = 0;
    }
    
    private void Update()
    {
        GenerateTargetableList();
        if (!FocusIsEnable)
        {
            if (_targetableAvailable.Count > 0)
            {
                CurrentTargetIndex = 0;
                if (CurrentTarget != _lastcachedTarget)
                {
                    Switch();
                }
            }
           
        }
    }
}
