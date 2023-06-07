using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlowMotionAttackBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private ScriptableValueListGameObject scriptableValueListGameObject;
    [SerializeField] private float temporaryTimeScale = 0.2f;
    [SerializeField] private float duration = 1f;
    private bool _isSlowAttack;
    private void OnEnable()
    {
        scriptableValueListGameObject.OnValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        scriptableValueListGameObject.OnValueChanged -= OnValueChanged;
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
        else if( obj.Count == 0 && playerCombat.canGiveDamage && playerCombat.DamageableWasAttackedAtThisFrame )
        {
            SetTimeScaleTemporary();
        }
    }

    public void SetTimeScaleTemporary()
    {
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
