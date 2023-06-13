using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlowMotionAttackBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerAttackManager playerAttackManager;
    [SerializeField] private ScriptableValueListGameObject scriptableValueListGameObject;
    [SerializeField] private bool _checkEnemyAwake = true;
    [SerializeField] private float temporaryTimeScale = 0.2f;
    [SerializeField] private float duration = 1f;
    public UnityEvent OnStartSlowMotion;
    
    private bool _isSlowAttack;
    private void OnEnable()
    {
        if(_checkEnemyAwake)
        {
            scriptableValueListGameObject.OnValueChanged += OnValueChanged;
        }
    }

    private void OnDisable()
    {
        if (_checkEnemyAwake)
        {
            scriptableValueListGameObject.OnValueChanged -= OnValueChanged;
        }
    }

    private void OnValueChanged(List<GameObject> obj)
    {
        if (obj.Count > 0 && _isSlowAttack)
        {
            if(!GameStateManager.Instance.pauseStateObject.activeSelf)
            {
                Time.timeScale = GameStateManager.Instance.GameStateOverride.timeScale;
            }
            _isSlowAttack = false;
        }
        else if( obj.Count <= 0 && playerAttackManager.canGiveDamage && playerAttackManager.DamageableWasAttackedAtThisFrame )
        {
            SetTimeScaleTemporary();
        }
    }

    public void SetTimeScaleTemporary()
    {
        OnStartSlowMotion?.Invoke();
        StartCoroutine(TimescaleCoroutine(temporaryTimeScale,duration));
    }
    IEnumerator TimescaleCoroutine(float value = 1, float duration = 0.2f)
    {
        _isSlowAttack = true;
        yield return new WaitForEndOfFrame();
        Time.timeScale = value;
        yield return new WaitForSecondsRealtime(duration);
        if(!GameStateManager.Instance.pauseStateObject.activeSelf)
        {
            Time.timeScale = GameStateManager.Instance.GameStateOverride.timeScale;
        }
        _isSlowAttack = false;
    }
}
