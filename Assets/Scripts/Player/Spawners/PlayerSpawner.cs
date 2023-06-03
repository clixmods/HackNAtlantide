using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private ScriptableValueVector3 playerPosition;
    [SerializeField] private List<Transform> playerSpawns;
    private PlayerMovement _playerMovement;

    [SerializeField] private bool savePlayerPositionOnDestroy = true;
    // Start is called before the first frame update
    void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        TeleportPlayerToSafeSpawn();

    }

    public void TeleportPlayerToSafeSpawn()
    {
        if (playerPosition.Value == Vector3.zero)
        {
            // No position saved so we keep the default spawn
            return;
        }
        // Find the nearest from the last player position
        Transform nearest = playerSpawns[0];
        for (int i = 1; i < playerSpawns.Count; i++)
        {
            float distanceWithNearest = Mathf.Infinity;
            float distanceWithNewOne = Mathf.Infinity;
            if (nearest.gameObject.activeSelf)
            {
                distanceWithNearest = Vector3.Distance(nearest.position, playerPosition.Value);
            }
            if (playerSpawns[i].gameObject.activeSelf)
            {
                distanceWithNewOne = Vector3.Distance(playerSpawns[i].position, playerPosition.Value);
            }
            
            if (distanceWithNewOne < distanceWithNearest)
            {
                nearest = playerSpawns[i];
            }
        }
        if(nearest.gameObject.activeSelf)
            _playerMovement.TeleportWorldSpawn(nearest.position);
    }

    private void OnDestroy()
    {
        if (savePlayerPositionOnDestroy)
        {
            SavePlayerPosition();
        }
    }
    [ContextMenu("Save Player Position")]
    public void SavePlayerPosition()
    {
        playerPosition.Value = _playerMovement.transform.position;
    }
}
