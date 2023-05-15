using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatTuto : MonoBehaviour
{
    private QTEHandler _qTEHandler;
    private Focus _focus;

    bool _hasDoneAttackQte = false;
    bool _hasDoneDashQte = false;
    bool _hasDoneFocusQte = false;
    bool _hasDoneDashAttackQte = false;

    bool _isInCutScene;
    bool _listenToEventAttack;
    private void Awake()
    {
        _focus = FindObjectOfType<Focus>();
        _qTEHandler = FindObjectOfType<QTEHandler>();
    }
    private void OnEnable()
    {
        GameStateManager.Instance.tutoStateObject.SetActive(true);
        _qTEHandler.ActiveAllInput(false);
        _qTEHandler.cutSceneSuccess += CutSceneSuccess;
    }
    private void OnDisable()
    {
        _qTEHandler.cutSceneSuccess -= CutSceneSuccess;
        GameStateManager.Instance.tutoStateObject.SetActive(false);
    }

    void CutSceneSuccess(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Interact:
                StartCoroutine(WaitForNewQTE());
                break;
            case InputType.Attack:
                _hasDoneAttackQte = true;
                _qTEHandler.CancelMove();
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
        if(_hasDoneAttackQte)
        {
            yield return new WaitForSeconds(1f);
        }
        _isInCutScene = false;
    }
    private void Update()
    {
        if(_hasDoneDashAttackQte && _hasDoneAttackQte && _hasDoneDashQte)
        {
            _qTEHandler.ActiveAllInput(true);
            Destroy(this);
        }
        if(_focus!=null && _focus.CurrentTarget != null && !_isInCutScene)
        {
            try
            {
                if (_focus.CurrentTarget.transform.TryGetComponent<EnemyController>(out EnemyController enemy) && !_listenToEventAttack)
                {
                   
                    enemy.GetComponent<Animator>().speed = 0.8f;
                    enemy._attackEvent += DashQTE;
                    _listenToEventAttack = true;
                }
            }
            catch
            {
                Debug.LogWarning("target is null");
            }
            
            //Focus
            if (!_hasDoneFocusQte)
            {
                Debug.Log("Dans le IF ");
                _qTEHandler.LaunchCutScene(InputType.Focus);
                _isInCutScene = true;
                _qTEHandler.MovePlayerToEnemy(_focus.CurrentTarget.transform.position);
                _qTEHandler.ActiveInputType(InputType.Move, false);
            }

            //Attack
            if (!_hasDoneAttackQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude <3.5f)
                {
                    _qTEHandler.ActiveInputType(InputType.Move, false);
                    _qTEHandler.CancelMove();
                    Debug.Log("QTE Attack");
                    _qTEHandler.LaunchCutScene(InputType.Attack);
                    _isInCutScene = true;
                }
            }
            //DashAttack
            if (!_hasDoneDashAttackQte && _hasDoneAttackQte && _hasDoneDashQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 5f && (_focus.CurrentTarget.transform.position - transform.position).magnitude > 2f)
                {
                    Debug.Log("QTE DashAttack");
                    _qTEHandler.ActiveInputType(InputType.Move, true);
                    _qTEHandler.LaunchCutScene(InputType.DashAttack);
                    _isInCutScene = true;
                }
            }
        }
        void DashQTE()
        {
            //Dash
            if (_focus.CurrentTarget != null && !_isInCutScene)
            {
                if (_hasDoneAttackQte && !_hasDoneDashQte && _focus.CurrentTarget.transform.TryGetComponent<EnemyController>(out EnemyController enemy))
                {
                    if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 5f)
                    {

                        Debug.Log("QTE Dash");
                        _qTEHandler.LaunchCutScene(InputType.Dash);
                        _qTEHandler.ActiveInputType(InputType.Move, true);
                        _isInCutScene = true;
                    }
                }
            }
        }

    }
}
