using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTutoDashAttack : MonoBehaviour
{
    private QTEHandler _qTEHandler;
    private Focus _focus;
    bool _hasDoneDashAttackQte = false;
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
        return _hasDoneDashAttackQte && _hasDoneFocusQte;
    }

    IEnumerator WaitForNewQTE()
    {
        _isInCutScene = true;
        yield return new WaitForSeconds(0.3f);
        _isInCutScene = false;
    }
    private void Update()
    {
        if (_hasDoneDashAttackQte)
        {
            _qTEHandler.ActiveAllInput(true);
            Destroy(this);
        }
        if (_focus != null && _focus.CurrentTarget != null && !_isInCutScene)
        {
            try
            {
                if (_focus.CurrentTarget.transform.TryGetComponent<EnemyAttackBehaviour>(out EnemyAttackBehaviour enemy) && !_listenToEventAttack)
                {

                    enemy.GetComponent<Animator>().speed = 0.8f;
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
            }
            //DashAttack
            if (!_hasDoneDashAttackQte)
            {
                if ((_focus.CurrentTarget.transform.position - transform.position).magnitude < 3.5f && (_focus.CurrentTarget.transform.position - transform.position).magnitude > 2f)
                {
                    _qTEHandler.LaunchCutScene(InputType.DashAttack);
                    _isInCutScene = true;
                }
            }
        }
    }
}
