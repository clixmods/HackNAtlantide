using System;
using System.Collections;
using System.Collections.Generic;
using Loading;
using UnityEngine;

public class CheckpointLoader : LoaderBehaviour
{
    [SerializeField] private DataPersistentHandler dataPersistentHandler;
    protected override void LoaderAction()
    {
        
    }

    protected override IEnumerator LoaderRoutine()
    {
        var playerSpawner = FindObjectOfType<PlayerSpawner>();
        if(playerSpawner != null)
            playerSpawner.SavePlayerPosition();
        
        dataPersistentHandler.SaveAll();
        yield return new WaitForSecondsRealtime(0.5f);
        LoaderEnd?.Invoke();
    }
}
