using System;
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
        private IDamageable _damageable;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _damageable.OnDamage += DamageableOnOnDamage;
        }

        private void DamageableOnOnDamage(object sender, EventArgs e)
        {
            for (int i = HealthEvents.Count - 1; i >= 0; i--)
            {
                if (_damageable.health/_damageable.maxHealth < HealthEvents[i].healthPercentageToLaunchEvent/100f)
                {
                    
                    HealthEvents[i].OnHealthPercentage?.Invoke();
                    HealthEvents.RemoveAt(i);
                    break;
                }
                
            }
        }
    }
}