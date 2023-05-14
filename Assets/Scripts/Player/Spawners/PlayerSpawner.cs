using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
   private PlayerMovement _playerMovement;
   private void Awake()
   {
      _playerMovement = FindObjectOfType<PlayerMovement>();
   }

   private void Start()
   {
      TeleportPlayerToSpawner();
   }

   public void TeleportPlayerToSpawner()
   {
      _playerMovement.Teleport(transform);
   }
}
