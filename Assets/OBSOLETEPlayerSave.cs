using System;
using System.Collections;
using System.Collections.Generic;
using _2DGame.Scripts.Save;
using UnityEngine;

public class OBSOLETEPlayerSave : MonoBehaviour
{
    // private PlayerHealth _playerHealth;
    // private PlayerStamina _playerStamina;
    // private void Awake()
    // {
    //     _playerHealth = GetComponent<PlayerHealth>();
    //     _playerStamina = GetComponent<PlayerStamina>();
    // }
    //
    // #region Saveable
    // class PlayerData : SaveData
    // {
    //     public float health;
    //     public float maxHealth;
    // }
    //
    // public override void OnLoad(string data)
    // {
    //     Debug.Log(data);
    //     PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
    //     _playerHealth.maxHealth = playerData.maxHealth;
    //     _playerHealth.health = playerData.health;
    // }
    //
    // public override void OnSave(out SaveData saveData)
    // {
    //     PlayerData playerData = new PlayerData();
    //     playerData.maxHealth = _playerHealth.maxHealth;
    //     playerData.health = _playerHealth.health;
    //     saveData = playerData;
    // }
    //
    // public override void OnReset()
    // {
    //     throw new System.NotImplementedException();
    // }
    // #endregion
}
