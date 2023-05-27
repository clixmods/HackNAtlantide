using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public enum InputType
{
    Interact,
    Attack,
    Dash,
    DashAttack,
    Focus,
    Move
}
public class QTEHandler : MonoBehaviour
{
    [SerializeField] private InputButtonScriptableObject _interact;
    [SerializeField] private InputButtonScriptableObject _attack;
    [SerializeField] private InputButtonScriptableObject _dash;
    [SerializeField] private InputButtonScriptableObject _dashAttack;
    [SerializeField] private InputButtonScriptableObject _focus;
    [SerializeField] private InputVectorScriptableObject _move;
    bool _hasInteract;
    bool _hasAttack;
    bool _hasDash;
    bool _hasDashAttack;
    bool _hasFocus;
    public bool HasFocus { get { return _hasFocus; } set { _hasFocus = value; } }
    Vector2 _moveInput;

    [SerializeField] GameObject _inputInfoAttack;
    [SerializeField] GameObject _inputInfoDash;
    [SerializeField] GameObject _inputInfoDashAttack;
    [SerializeField] GameObject _inputInfoInteract;
    [SerializeField] GameObject _inputInfoFocus;

    bool _isInCutScene = false;
    public Action<InputType> cutSceneSuccess;
    CinemachineBrain cine;
    CinemachineVirtualCamera VirtualCamera;
    [SerializeField] PostProcessWeightTransition _postProcessWeightTransition;

    void Start()
    {
        cine = FindObjectOfType<CinemachineBrain>();
        VirtualCamera = (CinemachineVirtualCamera)cine.ActiveVirtualCamera;
    }
    private void OnEnable()
    {
        DisableInputsInfo();
        _interact.OnValueChanged += InteractInput;
        _attack.OnValueChanged += AttackInput;
        _dashAttack.OnValueChanged += DashAttackInput;
        _focus.OnValueChanged += FocusInput;
        _dash.OnValueChanged += DashInput;
        _move.OnValueChanged += MoveInput;
    }
    private void OnDisable()
    {
        _postProcessWeightTransition.SetWeightVolume(0f);
    }
    void DisableInputsInfo()
    {
        _inputInfoAttack.SetActive(false);
        _inputInfoDash.SetActive(false);
        _inputInfoDashAttack.SetActive(false);
        _inputInfoInteract.SetActive(false);
        _inputInfoFocus.SetActive(false);
    }
    public void MoveInput(Vector2 input)
    {
        _moveInput = input;
    }
    public void InteractInput(bool value)
    {
        _hasInteract = value;
    }
    public void AttackInput(bool value)
    {
        _hasAttack = value;
    }
    public void DashInput(bool value)
    {
        _hasDash = value && _moveInput.magnitude > 0.5f;
    }
    public void DashAttackInput(bool value)
    {
        _hasDashAttack = value;
    }
    public void FocusInput(bool value)
    {
        _hasFocus = value;
    }
    public void LaunchCutScene(InputType inputType)
    {
        if(!_isInCutScene)
        {
            StartCoroutine(Cutscene(inputType));
        }
    }
    IEnumerator Cutscene(InputType inputType)
    {
        _isInCutScene = true;
       
        //_postProcessWeightTransition.SetWeightVolume(1f);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 0.2f).SetUpdate(true);

        ActiveInputType(inputType, true);
        

        while (!CutSceneFinish(inputType))
        {
            yield return null;
        }
        ActiveInputType(inputType, false);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.2f).SetUpdate(true);
        //_postProcessWeightTransition.SetWeightVolume(0f);
        _isInCutScene = false;
        cutSceneSuccess?.Invoke(inputType);
        DisableInputsInfo();
    }
    public void ActiveInputType(InputType inputType, bool active)
    {
        switch (inputType)
        {
            case InputType.Interact:
                //_interact.IsActive = active;
                break;

            case InputType.Attack:
                _attack.IsActive = active;
                break;

            case InputType.Dash:
                _dash.IsActive = active;
                break;

            case InputType.DashAttack:
                _dashAttack.IsActive = active;
                break;

            case InputType.Focus:
                _focus.IsActive = true;
                break;

            case InputType.Move:
                _move.IsActive = active;
                break;

            default:
                break;
        }
    }
    public void CancelMove()
    {
        FindObjectOfType<PlayerMovement>().CancelMove();
    }
    public void MovePlayerToEnemy(Vector3 Target)
    {
        FindObjectOfType<PlayerMovement>().MoveTo(Target);
    }

    public void ActiveAllInput(bool active)
    {
        //_interact.IsActive = active;
        _attack.IsActive = active;
        _dash.IsActive = active;
        _dashAttack.IsActive = active;
        _focus.IsActive = active;
    }
    bool CutSceneFinish(InputType inputType)
    {
        bool value = false;
        switch (inputType)
        {
            case InputType.Interact:
                value = _hasInteract;
                _inputInfoInteract.SetActive(true);
                break;

            case InputType.Attack:
                value = _hasAttack;
                _inputInfoAttack.SetActive(true);
                break;

            case InputType.Dash:
                value = _hasDash;
                _inputInfoDash.SetActive(true);
                break;

            case InputType.DashAttack:
                value = _hasDashAttack;
                _inputInfoDashAttack.SetActive(true);
                break;

            case InputType.Focus:
                value = _hasFocus;
                _inputInfoFocus.SetActive(true);
                break;

            default:
                value = false;
                break;
        }
        return value;
    }
}
