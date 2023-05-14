using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSpawnersCondition : MonoBehaviour
{
    [Serializable]
    private struct PlayerSpawnCondition
    {
         public ScriptableValueBool playerSpawnerCondition;
         public Transform playerSpawner;
    }
    [SerializeField] private PlayerSpawnCondition[] playerSpawnersCondition;
    // Start is called before the first frame update
    void Start()
    {
        // Disable all spawners
        for (int i = 0; i < playerSpawnersCondition.Length; i++)
        {
            playerSpawnersCondition[i].playerSpawner.gameObject.SetActive(false);
        }

        bool isSpawned = false;
        // Enable only one
        for (int i = playerSpawnersCondition.Length - 1; i >= 0; i--)
        {
            if (playerSpawnersCondition[i].playerSpawnerCondition.Value)
            {
                playerSpawnersCondition[i].playerSpawner.gameObject.SetActive(true);
                isSpawned = true;
                break;
            }
        }

        if (!isSpawned)
        {
            if (playerSpawnersCondition != null && playerSpawnersCondition.Length > 0)
            {
                playerSpawnersCondition[0].playerSpawner.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
