using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UITransformFollower
{
    public class UIHealthBarTransformFollower : UITransformFollower
    {
        private IDamageable idamageable;
        private float _maxHealth;
        private Slider _slider;
        public override void Create()
        {
            base.Create();
            
        }
        public void Init(IDamageable idamageable1)
        {
            idamageable = idamageable1;
            idamageable.OnDamage += IdamageableOnDamage;
            idamageable.OnDeath += IdamageableOnDeath;
            
            _slider ??= GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            _maxHealth = idamageable.health;
        }

        private void IdamageableOnDeath(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        private void IdamageableOnDamage(object sender, EventArgs e)
        {
            _slider.value = idamageable.health / idamageable.maxHealth;
            Debug.Log(idamageable.health /idamageable.maxHealth);
        }
        
    }
}