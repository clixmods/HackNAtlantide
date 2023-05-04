using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UITransformFollower
{
    public class UIAttackInteractableTransformFollower : UITransformFollower
    {
        // private AttackLevitationInteractable idamageable;
        // public override void Create()
        // {
        //     base.Create();
        //     
        // }
        //
        // private void Awake()
        // {
        //     _slider = GetComponentInChildren<Slider>();
        //     _rectTransform = _slider.transform.GetComponent (typeof (RectTransform)) as RectTransform;
        // }
        //
        // public void Init(IDamageable idamageable1)
        // {
        //     idamageable = idamageable1;
        //     idamageable.OnDamage += IdamageableOnDamage;
        //     idamageable.OnDeath += IdamageableOnDeath;
        //     idamageable.OnChangeHealth += IdamageableOnOnChangeHealth;
        //     
        //     _slider ??= GetComponentInChildren<Slider>();
        //     _striLength = _rectTransform.sizeDelta.x;
        //     _rectTransform.sizeDelta = new Vector2 (_striLength * idamageable.maxHealth, _rectTransform.sizeDelta.y);
        // }
        //
        // private void IdamageableOnOnChangeHealth(object sender, EventArgs e)
        // {
        //     _rectTransform.sizeDelta = new Vector2 (_striLength * idamageable.maxHealth, _rectTransform.sizeDelta.y);
        // }
        //
        // private void Start()
        // {
        //     _maxHealth = idamageable.health;
        // }
        //
        // private void IdamageableOnDeath(object sender, EventArgs e)
        // {
        //     Destroy(gameObject);
        // }
        //
        // private void IdamageableOnDamage(object sender, EventArgs e)
        // {
        //     _slider.value = idamageable.health / idamageable.maxHealth;
        //     Debug.Log(idamageable.health /idamageable.maxHealth);
        // }
        //
    }
}