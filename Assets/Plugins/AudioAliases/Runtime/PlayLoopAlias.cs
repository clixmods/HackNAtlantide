using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayLoopAlias : MonoBehaviour
{
    [SerializeField] private Alias aliasToPlay;
    private AudioPlayer _audioPlayer;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            if (aliasToPlay != null)
            {
                gameObject.name = $"Play Loop : {aliasToPlay.name}";
            }
        #endif
    }

    private void OnEnable()
    {
        PlayAlias();
    }

    private void OnDestroy()
    {
        StopAlias();
    }

    private void OnDisable()
    {
        StopAlias();
    }

    public void PlayAlias()
    {
        transform.PlayLoopSound(aliasToPlay,ref _audioPlayer );
    }
    
    public void StopAlias()
    {
        _audioPlayer.StopSound(StopLoopBehavior.Direct);
    }
}
