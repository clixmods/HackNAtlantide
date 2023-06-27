using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Damageable
{
    [Serializable]
    public struct HealthEvent
    {
        [Range(0,100f)]
        public float healthPercentageToLaunchEvent;
        
        public UnityEvent OnHealthPercentage;
    }
    public class DamageableHealthEvents : MonoBehaviour
    {
        [SerializeField] private List<HealthEvent> HealthEvents;
        private EnemyBehaviour _enemyBehaviour;
        private EnemyHealth _enemyHealth;
        private IDamageable _damageable;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _damageable.OnDamage += DamageableOnOnDamage;
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _enemyHealth = GetComponent<EnemyHealth>();
        } 

        private void DamageableOnOnDamage(object sender, EventArgs e)
        {
            for (int i = HealthEvents.Count - 1; i >= 0; i--)
            {
                if (_damageable.health/_damageable.maxHealth < HealthEvents[i].healthPercentageToLaunchEvent/100f)
                {
                    StartCoroutine(StartEvent(HealthEvents[i]));
                    _enemyHealth.SetInvulnerability(true);
                    HealthEvents.RemoveAt(i);
                    break;
                }
            }
        }

        IEnumerator StartEvent(HealthEvent eventToLaunch)
        {
            while (true)
            {
                if (_enemyBehaviour.IsAttacking || !_enemyBehaviour.IsAwake || _enemyBehaviour.ReturnToStartPos)
                {
                    yield return null;
                }
                else
                {
                    if (_enemyBehaviour.CurrentAttack != null)
                    {
                        _enemyBehaviour.CurrentAttack.CancelAttack();
                    }
                    eventToLaunch.OnHealthPercentage?.Invoke();
                    yield break;
                }
            }
        }
    }
}