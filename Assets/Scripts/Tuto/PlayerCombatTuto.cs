using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTuto : MonoBehaviour
{
    private QTEHandler _qTEHandler;
    private Focus _focus;

    bool _hasDoneAttackQte = false;
    bool _hasDoneDashQte = false;
    bool _hasDoneFocusQte = false;

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
            yield return new WaitForSeconds(0.3f);
        }
        _isInCutScene = false;
    }
    private void Update()
    {
        if(_hasDoneAttackQte && _hasDoneDashQte)
        {
            _qTEHandler.ActiveAllInput(true);
            Destroy(this);
        }
        if(_focus!=null && _focus.CurrentTarget != null && !_isInCutScene)
        {
            try
            {
                if (_focus.CurrentTarget.transform.TryGetComponent<EnemyAttackBehaviour>(out EnemyAttackBehaviour enemy) && !_listenToEventAttack)
                {
                   
                    enemy.GetComponent<Animator>().speed = 0.8f;
                    enemy.OnAttack += DashQTE;
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
                _qTEHandler.LaunchCutScene(InputType.Focus);
                _isInCutScene = true;
                /*_qTEHandler.ActiveInputType(InputType.Move, false);
                _qTEHandler.MovePlayerToEnemy(_focus.CurrentTarget.transform.position);*/
            }

            //Attack
            if (!_hasDoneAttackQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude <3.5f)
                {
                    _qTEHandler.CancelMove();
                    _qTEHandler.LaunchCutScene(InputType.Attack);
                    _isInCutScene = true;
                }
            }
        }
        void DashQTE()
        {
            //Dash
            if (!_isInCutScene)
            {
                if (_hasDoneAttackQte && !_hasDoneDashQte)
                {
                    if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 5f)
                    {
                        _qTEHandler.LaunchCutScene(InputType.Dash);
                        _qTEHandler.ActiveInputType(InputType.Move, true);
                        _isInCutScene = true;
                    }
                }
            }
        }

    }
}
