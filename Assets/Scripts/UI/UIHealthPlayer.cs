using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPlayer : MonoBehaviour
{
    [SerializeField] private ScriptableValueFloat healthValue;
    private Slider _slider;
    // Calcul maxhealth length
    private float _striLength;
    private RectTransform _rectTransform;
    private void Awake()
    {
        
        _slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent<RectTransform>();
        _striLength = _rectTransform.rect.width;
        healthValue.OnValueChanged += OnValueChanged;
        _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.Value01;
        
    }

    private void OnValueChanged(float currentValue)
    {
        _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.Value01;
    }

    private void Start()
    {
        // player = PlayerInstanceScriptableObject.Player.GetComponent<IDamageable>();
        // if (player != null)
        // {
        //     player.OnDamage += PlayerOnDamage;
        //     player.OnChangeHealth += PlayerOnOnChangeHealth;
        //
        //     _striLength = _rectTransform.rect.width;
        //     _rectTransform.sizeDelta = new Vector2 (_striLength * player.maxHealth, _rectTransform.sizeDelta.y);
        //     _slider.value = player.health/player.maxHealth;
        // }
    }

    private void OnDestroy()
    {
        healthValue.OnValueChanged -= OnValueChanged;
        // if (player != null)
        // {
        //     player.OnDamage -= PlayerOnDamage;
        //     player.OnChangeHealth -= PlayerOnOnChangeHealth;
        // }
    }

    private void PlayerOnOnChangeHealth(object sender, EventArgs e)
    {
        // _rectTransform.sizeDelta = new Vector2 (_striLength * player.maxHealth, _rectTransform.sizeDelta.y);
        // _slider.value = player.health/player.maxHealth;
    }

    private void PlayerOnDamage(object sender, EventArgs e)
    {
        // _slider.value = player.health/player.maxHealth;
    }

}
