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
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        player = PlayerInstanceScriptableObject.Player.GetComponent<IDamageable>();
        player.OnDamage += PlayerOnDamage;
    }

    private void PlayerOnDamage(object sender, EventArgs e)
    {
        _slider.value = player.health/player.maxHealth;
    }
    
}
