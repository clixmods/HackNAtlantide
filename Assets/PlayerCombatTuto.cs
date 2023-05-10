using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatTuto : MonoBehaviour
{
    [SerializeField] private QTEHandler _qTEHandler;
    [SerializeField] private Focus _focus;
    PlayerDetectionScriptableObject _playerDetectionScriptableObject;

    bool _hasDoneAttackQte = false;
    bool _hasDoneDashQte = false;
    bool _hasDoneFocusQte = false;
    bool _hasDoneDashAttackQte = false;

    bool _isInCutScene;
    bool isAttacking;
    InputType _lastInputTypeDone;
    private void Awake()
    {
        _focus = FindObjectOfType<Focus>();
        _qTEHandler = FindObjectOfType<QTEHandler>();
    }
    private void OnEnable()
    {
        _qTEHandler.cutSceneSuccess += CutSceneSuccess;
    }
    private void OnDisable()
    {
        _qTEHandler.cutSceneSuccess -= CutSceneSuccess;
    }

    void CutSceneSuccess(InputType inputType)
    {
        _lastInputTypeDone = inputType;
        switch (inputType)
        {
            case InputType.Interact:
                StartCoroutine(WaitForNewQTE());
                break;
            case InputType.Attack:
                _hasDoneAttackQte = true;
                StartCoroutine(WaitForNewQTE());
                break;
            case InputType.Dash:
                StartCoroutine(WaitForNewQTE());
                _hasDoneDashQte = true;
                break;
            case InputType.DashAttack:
                _hasDoneDashAttackQte = true;
                StartCoroutine(WaitForNewQTE());
                break;
            case InputType.Focus:
                StartCoroutine(WaitForNewQTE());
                _hasDoneFocusQte = true;
                break;
        }
    }

    bool HasFinishedTuto()
    {
        return _hasDoneAttackQte && _hasDoneDashQte && _hasDoneFocusQte;
    }
    IEnumerator WaitForNewQTE()
    {
        _isInCutScene = true;
        yield return new WaitForSeconds(2f);
        _isInCutScene = false;

    }
    private void Update()
    {
        Debug.Log(_hasDoneAttackQte);
        if(_focus.CurrentTarget != null && !_isInCutScene)
        {
            //Focus
            if (!_hasDoneFocusQte)
            {
                _qTEHandler.LaunchCutScene(InputType.Focus);
                _isInCutScene = true;
            }

            //Attack
            if (!_hasDoneAttackQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 2.5f)
                {
                    Debug.Log("QTE Attack");
                    _qTEHandler.LaunchCutScene(InputType.Attack);
                    _isInCutScene = true;
                }
            }

            //Dash
            if (_hasDoneAttackQte && !_hasDoneDashQte && _focus.CurrentTarget.transform.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 5f && enemy.IsAttacking)
                {
                    Debug.Log("QTE Dash");
                    _qTEHandler.LaunchCutScene(InputType.Dash);
                    _isInCutScene = true;
                }
            }

            //DashAttack
            if (!_hasDoneDashAttackQte && _hasDoneAttackQte && _hasDoneDashQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 5f && (_focus.CurrentTarget.transform.position - transform.position).magnitude > 2f)
                {
                    Debug.Log("QTE DashAttack");
                    _qTEHandler.LaunchCutScene(InputType.DashAttack);
                    _isInCutScene = true;
                }
            }
        }
        
    }
}
