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

    [Header("Input")] [Tooltip("Input to enable and disable focus mode")] [SerializeField]
    private InputButtonScriptableObject inputEnableFocus;

    [Tooltip("Input Vector to switch between the list of target")] [SerializeField]
    private InputVectorScriptableObject inputSwitchTarget;

    private List<IFocusable> _FocusableAvailable = new List<IFocusable>();

    [SerializeField] CinemachineVirtualCamera cameraVirtualFocus;
    private Transform _camFocusTransform;
    private FocusCinemachineTargetGroup _cinemachineTargetGroup;
    private int _currentTargetIndex;
    private CinemachineVirtualCamera _noFocusVirtualCamera;
    private IFocusable _lastCachedTarget;
    private bool _isTransitioning;
    private bool _inputFocusIsPressed;

    #region Properties

    public List<IFocusable> FocusablesAvailable => _FocusableAvailable;

    public IFocusable CurrentTarget
    {
        get
        {
            GenerateTargetableList();
            if (_FocusableAvailable.Count == 0)
                return null;

            return _FocusableAvailable[CurrentTargetIndex];
        }
    }

    private int CurrentTargetIndex
    {
        get
        {
            if (_currentTargetIndex > _FocusableAvailable.Count - 1)
            {
                _currentTargetIndex = _FocusableAvailable.Count - 1;
            }

            return _currentTargetIndex;
        }
        set
        {
            if (_FocusableAvailable.Count == 0)
            {
                _currentTargetIndex = 0;
                return;
            }

            _currentTargetIndex = value;
            if (_currentTargetIndex < 0)
            {
                _currentTargetIndex = _FocusableAvailable.Count - 1;
            }
            else if (_currentTargetIndex > _FocusableAvailable.Count - 1)
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

            _camFocusTransform.position = _noFocusVirtualCamera.State.FinalPosition;
            _camFocusTransform.rotation = _noFocusVirtualCamera.State.FinalOrientation;
        }
    }

    private void Update()
    {
        GenerateTargetableList();
        AfterGenerateList();
        if (FocusIsEnable)
        {
            //_camFocusTransform.position = _noFocusVirtualCamera.transform.position ;
            //_camFocusTransform.rotation =  _noFocusVirtualCamera.transform.rotation;
        }
        else
        {
            if (_FocusableAvailable.Count > 0)
            {
                _FocusableAvailable[0].OnNearest();
            }
        }
    }

    #endregion

    private void OnCameraTransitionChange(CinemachineVirtualCamera newCameraVirtual)
    {
        if (FocusIsEnable)
        {
            // If a new camera transition is triggered, go lerp the focus camera to the new camera
            if (newCameraVirtual != _noFocusVirtualCamera)
            {
                // we need to cache the camera transition
                _noFocusVirtualCamera = newCameraVirtual;
                StartCoroutine(LerpCameraPositionTo(newCameraVirtual));
            }
        }

        // we need to cache the camera transition
        _noFocusVirtualCamera = newCameraVirtual;
    }

    IEnumerator LerpCameraPositionTo(CinemachineVirtualCamera newCameraVirtual)
    {
        _isTransitioning = true;
        float timeElapsed = 0;
        Vector3 cameraPosition = cameraVirtualFocus.State.FinalPosition;
        Quaternion cameraRotation = cameraVirtualFocus.State.FinalOrientation;
        float timeTransition = 2;
        while (timeElapsed < timeTransition)
        {
            timeElapsed += Time.deltaTime;
            if (newCameraVirtual != _noFocusVirtualCamera)
            {
                _isTransitioning = false;
                yield break;
            }

            var t = timeElapsed / timeTransition;
            _camFocusTransform.position = Vector3.Lerp(cameraPosition, newCameraVirtual.State.FinalPosition, t);
            _camFocusTransform.rotation = Quaternion.Lerp(cameraRotation, newCameraVirtual.State.FinalOrientation, t);
            yield return new WaitForFixedUpdate();
        }

        _isTransitioning = false;
    }

    private void Switch()
    {
        // No target available
        if (_FocusableAvailable.Count == 0)
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
        if (FocusIsEnable)
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
        if (InputManager.IsGamepad())
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

        Vector3 currentTargetPos = CurrentTarget.transform.position;
        int index = 0;
        float closestdot = -2;
        for (int i = 0; i < _FocusableAvailable.Count; i++)
        {
            float dot = Vector3.Dot(inputDirection, (_FocusableAvailable[i].transform.position - currentTargetPos).normalized);

            if (dot > closestdot)
            {
                index = i;
                closestdot = dot;
            }
        }

        return index;
    }

    IEnumerator TryFocus()
    {
        while (_inputFocusIsPressed)
        {
            //TryFocus
            if (CanFocus())
            {
                // Focus will be enabled
                if (!FocusIsEnable)
                {
                    FocusIsEnable = true;
                    CurrentTargetIndex = 0;
                    // Active the camera focus
                    cameraVirtualFocus.gameObject.SetActive(true);
                    // go disable the nofocus camera
                    OnFocusEnable?.Invoke();
                    Switch();
                    try
                    {
                        if (CurrentTarget != null)
                            CurrentTarget.OnFocus();
                    }
                    catch
                    {
                        Debug.LogWarning(
                            "A IFocusable has been destroyed ! Its better to not destroy them in a same scene");
                    }
                }
            }
            else if (FocusIsEnable)
            {
                DisableFocus();
            }

            yield return null;
        }

        //CancelTryFocus
        DisableFocus();
    }

    private void InputEnableFocusOnChanged(bool value)
    {
        _inputFocusIsPressed = value;
        if (value)
        {
            StartCoroutine(TryFocus());
        }
    }

    private bool CanFocus()
    {
        GenerateTargetableList();
        AfterGenerateList();
        return _FocusableAvailable.Count > 0;
    }

    private bool _forceSwitch = false;

    private void GenerateTargetableList()
    {
        // If no targetables is available in the scene, we need to check if targetableavailable is not fucked
        if (IFocusable.Focusables.Count == 0)
        {
            if (_FocusableAvailable.Count > 0)
            {
                _FocusableAvailable = new List<IFocusable>();
            }

            DisableFocus();
            return;
        }
        else
        {
            foreach (var targetable in IFocusable.Focusables)
            {
                targetable.OnNoNearest();
            }
        }

        // Generate list of available targetable
        _FocusableAvailable = new List<IFocusable>();
        foreach (var targetable in IFocusable.Focusables.Where(targetable => targetable.CanBeFocusable))
        {
           
            _FocusableAvailable.Add(targetable);
        }

        // If the lastest target is not in the list, we need to force a switch
        if (!_FocusableAvailable.Contains(_lastCachedTarget))
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
        if (!FocusIsEnable && _FocusableAvailable.Count > 0)
        {
            Switch();
        }
    }

    private void SortTargetableAvailableListByNearest()
    {
        // Sort the list to have the nearest in first
        _FocusableAvailable.Sort(delegate(IFocusable t1, IFocusable t2)
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
        if (_noFocusVirtualCamera != null)
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