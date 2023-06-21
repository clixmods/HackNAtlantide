using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBoss : MonoBehaviour
{
    [SerializeField] private EnemyHealth healthValue;
    private Slider _slider;
    // Calcul maxhealth length
    private float _striLength;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent<RectTransform>();
        _striLength = _rectTransform.rect.width;
        healthValue.OnChangeHealth += OnValueChanged;
        //_rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.maxHealth, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.health / healthValue.maxHealth;
    }
    private void MaxValueChanged(float currentMaxValue)
    {
        //_rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.maxHealth, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.health / healthValue.maxHealth;;
    }

    private void OnValueChanged(float health, float maxHealth)
    {
        //_rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.maxHealth, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.health / healthValue.maxHealth;;
    }

    private void OnDestroy()
    {
        healthValue.OnChangeHealth -= OnValueChanged;
        
    }
}
