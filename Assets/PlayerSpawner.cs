using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private ScriptableValueVector3 playerPosition;
    [SerializeField] private List<Transform> playerSpawns;
    private PlayerMovement _playerMovement;
    // Start is called before the first frame update
    void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        
    }

    private void Start()
    {
        // Find the nearest from the last player position
        Transform nearest = playerSpawns[0];
        for (int i = 1; i < playerSpawns.Count; i++)
        {
            var distanceWithNearest = Vector3.Distance(nearest.position, playerPosition.Value);
            var distanceWithNewOne = Vector3.Distance(playerSpawns[i].position, playerPosition.Value);
            if (distanceWithNewOne < distanceWithNearest)
            {
                nearest = playerSpawns[i];
            }
        }
        _playerMovement.TeleportWorldSpawn(nearest.position);
    }

    private void OnDestroy()
    {
        playerPosition.Value = _playerMovement.transform.position;
    }
}
