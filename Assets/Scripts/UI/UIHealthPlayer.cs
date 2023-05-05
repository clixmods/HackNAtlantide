using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInstanceScriptableObject _playerInstanceScriptableObject;
    private Slider _slider;
    private IDamageable player;
    // Calcul maxhealth length
    private float _striLength;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent (typeof (RectTransform)) as RectTransform;
    }

    private void Start()
    {
        player = PlayerInstanceScriptableObject.Player.GetComponent<IDamageable>();
        player.OnDamage += PlayerOnDamage;
        player.OnChangeHealth += PlayerOnOnChangeHealth;
        
        _striLength = ((RectTransform)transform).rect.width;
        _rectTransform.sizeDelta = new Vector2 (_striLength * player.maxHealth, _rectTransform.sizeDelta.y);
    }

    private void PlayerOnOnChangeHealth(object sender, EventArgs e)
    {
        _rectTransform.sizeDelta = new Vector2 (_striLength * player.maxHealth, _rectTransform.sizeDelta.y);
    }

    private void PlayerOnDamage(object sender, EventArgs e)
    {
        _slider.value = player.health/player.maxHealth;
    }
    
}
