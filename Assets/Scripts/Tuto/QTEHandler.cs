using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum InputType
{
    Interact,
    Attack,
    Dash,
    DashAttack,
    Focus
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
    Vector2 _moveInput;

    [SerializeField] GameObject _inputInfoAttack;
    [SerializeField] GameObject _inputInfoDash;
    [SerializeField] GameObject _inputInfoDashAttack;
    [SerializeField] GameObject _inputInfoInteract;
    [SerializeField] GameObject _inputInfoFocus;


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
        _hasDash = value && _moveInput.magnitude>0.5f;
    }
    public void DashAttackInput(bool value)
    {
        _hasDashAttack = value;
    }
    public void FocusInput(bool value)
    {
        _hasFocus = value;
    }
    public void LaunchCutScene(int inputType)
    {
        StartCoroutine(Cutscene((InputType)inputType));
    }
    IEnumerator Cutscene(InputType inputType)
    {
        Time.timeScale = 0f;
        while(!CutSceneFinish(inputType))
        {
            yield return null;
        }
        Time.timeScale = 1f;

        DisableInputsInfo();
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
