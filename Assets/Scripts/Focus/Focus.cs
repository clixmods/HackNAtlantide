using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
public class Focus : MonoBehaviour
{
    public static bool FocusIsEnable;
    #region Events
    public delegate void EventFocus();
    public delegate void EventSwitchFocus(IFocusable target);
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
    
    private List<IFocusable> _targetableAvailable = new List<IFocusable>();
    
    [SerializeField] CinemachineVirtualCamera cameraVirtualFocus;
    private Transform _camFocusTransform;
    private FocusCinemachineTargetGroup _cinemachineTargetGroup;
    private int _currentTargetIndex;
    private GameObject _noFocusVirtualCamera;
    private IFocusable _lastCachedTarget;
    private bool _isTransitioning;
    #region Properties
    public IFocusable CurrentTarget
    {
        get
        {
            GenerateTargetableList();
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
    #region Monobehaviour
    private void Awake()
    {
        // focus is disable in the awake
        FocusIsEnable = false;
        // Input Behaviour
        inputEnableFocus.OnValueChanged += InputEnableFocusOnChanged;
        inputSwitchTarget.OnValueChanged += InputSwitchTargetOnChanged;
        // Check camera transition
        CinemachineCameraVirtualTransition.OnPostCameraChanged += OnCameraTransitionChange;
        // Setup target group
        _cinemachineTargetGroup = GetComponent<FocusCinemachineTargetGroup>();
        if (cameraVirtualFocus != null)
        {
            _camFocusTransform = cameraVirtualFocus.transform;
        }
    }
    private void OnDestroy()
    {
        DisableFocus();
        inputEnableFocus.OnValueChanged -= InputEnableFocusOnChanged;
        inputSwitchTarget.OnValueChanged -= InputSwitchTargetOnChanged;
        CinemachineCameraVirtualTransition.OnPostCameraChanged -= OnCameraTransitionChange;
    }
    private void Start()
    {
        DisableFocus();
    }
    private void FixedUpdate()
    {
        if (FocusIsEnable && _noFocusVirtualCamera != null && !_isTransitioning)
        {
           // Debug.Log("Camera Move");
            _camFocusTransform.position = _noFocusVirtualCamera.transform.position;
            _camFocusTransform.rotation = _noFocusVirtualCamera.transform.rotation;
        }
    }

    private void Update()
    {
        
        if (FocusIsEnable)
        {
            //_camFocusTransform.position = _noFocusVirtualCamera.transform.position ;
            //_camFocusTransform.rotation =  _noFocusVirtualCamera.transform.rotation;
            GenerateTargetableList();
            AfterGenerateList();
        }
    }
    #endregion
    private void OnCameraTransitionChange(CinemachineVirtualCamera newCameraVirtual)
    {
        if (FocusIsEnable)
        {
            // If a new camera transition is triggered, go lerp the focus camera to the new camera
            if (newCameraVirtual.gameObject != _noFocusVirtualCamera)
            {
                // we need to cache the camera transition
                _noFocusVirtualCamera = newCameraVirtual.gameObject;
               // if (!_isTransitioning)
                {
                    StartCoroutine(LerpCameraPositionTo(newCameraVirtual));
                }
               
             
            }
            else
            {
                // Focus enabled, we need to disable the new camera transition
                //newCameraVirtual.gameObject.SetActive(false); 
            }
        }
        else
        {
            var newCamTransitionTransform = newCameraVirtual.transform;
            //_camFocusTransform.position = newCamTransitionTransform.position;
            //_camFocusTransform.rotation = newCamTransitionTransform.rotation;
        }
        // we need to cache the camera transition
        _noFocusVirtualCamera = newCameraVirtual.gameObject;
        
        
        // // If its the same camera, not necesary to continue
        // if (newCameraVirtual.gameObject == _noFocusVirtualCamera)
        // {
        //     // Need to disable it, because the camera transition event has renable it
        //     _noFocusVirtualCamera.SetActive(false); 
        //     return;
        // }
    }
    IEnumerator LerpCameraPositionTo(CinemachineVirtualCamera newCameraVirtual)
    {
        _isTransitioning = true;
        float timeElapsed = 0;
        Vector3 cameraPosition = _camFocusTransform.position;
        Quaternion cameraRotation = _camFocusTransform.rotation;
        float timeTransition = 2;
        while (timeElapsed < timeTransition)
        {
            timeElapsed += Time.deltaTime;
            if (newCameraVirtual.gameObject != _noFocusVirtualCamera)
            {
                _isTransitioning = false;
                yield break;
            }
                
            var t = timeElapsed / timeTransition;
            _camFocusTransform.position = Vector3.Lerp(cameraPosition,newCameraVirtual.transform.position , t);
            _camFocusTransform.rotation = Quaternion.Lerp(cameraRotation, newCameraVirtual.transform.rotation, t);
            yield return null;
        }

        _isTransitioning = false;
    }
    private void Switch()
    {
        // No target available
        if (_targetableAvailable.Count == 0)
        {
            OnFocusNoTarget?.Invoke();
            DisableFocus();
            return;
        }
        // If the CurrentTarget is the last target, go return
        if (CurrentTarget != null && _lastCachedTarget == CurrentTarget)
        {
            OnFocusSwitch?.Invoke(CurrentTarget);
            return;
        }
        // When the focus is enable, we need to onuntarget the last target 
        if (FocusIsEnable )
        {
            try
            {
                _lastCachedTarget.OnUnfocus();
            }
            catch
            {
                Debug.LogWarning("A IFocusable has been destroyed ! Its better to not destroy them in a same scene");
                _lastCachedTarget = null;
            }
        }
        
        OnFocusSwitch?.Invoke(CurrentTarget);
        if (CurrentTarget != null)
        {
            _cinemachineTargetGroup.SwitchToTarget(CurrentTarget.focusableTransform);
            _lastCachedTarget = CurrentTarget;
            if (FocusIsEnable)
            {
                CurrentTarget.OnFocus();
            }
      
        }
    }
    private void InputSwitchTargetOnChanged(Vector2 value)
    {
        if (value.magnitude <= 0.7) return;
        
        GenerateTargetableList();
        AfterGenerateList();
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
    private int ClosestDotIndex(Vector2 inputValue)
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
            if (CanFocus())
            {
                FocusIsEnable = !FocusIsEnable;
                // Focus will be enabled
                if (FocusIsEnable)
                {
                    CurrentTargetIndex = 0;
                    // Active the camera focus
                    cameraVirtualFocus.gameObject.SetActive(true);
                    // go disable the nofocus camera
                    if (_noFocusVirtualCamera != null)
                    {
                        //_noFocusVirtualCamera.SetActive(false);
                    }
                    OnFocusEnable?.Invoke();
                    Switch();
                    try
                    {
                        if(CurrentTarget != null)
                            CurrentTarget.OnFocus();
                    }
                    catch
                    {
                        Debug.LogWarning("A IFocusable has been destroyed ! Its better to not destroy them in a same scene");
                    }
                
                }
                else
                {
                    DisableFocus();
                }
            }
            else if (FocusIsEnable)
            {
                FocusIsEnable = false;
                DisableFocus();
            }
        }
        
    }
    private bool CanFocus()
    {
        GenerateTargetableList();
        AfterGenerateList();
        return _targetableAvailable.Count > 0;
    }
    private bool _forceSwitch = false;
    private void GenerateTargetableList()
    {
        // If no targetables is available in the scene, we need to check if targetableavailable is not fucked
        if (IFocusable.Focusables.Count == 0)
        {
            if (_targetableAvailable.Count > 0)
            {
                _targetableAvailable = new List<IFocusable>();
            }
            DisableFocus();
            return; 
        }
        // Generate list of available targetable
        _targetableAvailable = new List<IFocusable>();
        foreach (var targetable in IFocusable.Focusables.Where(targetable => targetable.CanBeFocusable))
        {
            _targetableAvailable.Add(targetable);
        }
        // If the lastest target is not in the list, we need to force a switch
        if (!_targetableAvailable.Contains(_lastCachedTarget))
        {
            _forceSwitch = true;
        }
        SortTargetableAvailableListByNearest();
        
    }

    private void AfterGenerateList()
    {
        // After a sort by nearest, we can do the switch
        if (_forceSwitch)
        {
            Switch();
            _forceSwitch = false;
        }
        // TODO : Maybe a rewrite here ?
        if (!FocusIsEnable && _targetableAvailable.Count > 0)
        {
            Switch();
        }
    }
    private void SortTargetableAvailableListByNearest()
    {
        // Sort the list to have the nearest in first
        _targetableAvailable.Sort(delegate(IFocusable t1, IFocusable t2)
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
        OnFocusDisable?.Invoke();
        // Active the no focus camera cached
        if ( _noFocusVirtualCamera != null)
        {
           // _noFocusVirtualCamera.SetActive(true);
        }
        // Disable focus camera
        cameraVirtualFocus.gameObject.SetActive(false);
        FocusIsEnable = false;
        // Try / Catch used to prevent Interface field issues, Unity have a bad behaviour
        try
        {
            _lastCachedTarget.OnUnfocus();
            _lastCachedTarget = null;
        }
        catch
        {
            _lastCachedTarget = null;
           // Debug.LogWarning("A IFocusable has been destroyed ! Its better to not destroy them in a same scene");
        }
        CurrentTargetIndex = 0;
    }
}
