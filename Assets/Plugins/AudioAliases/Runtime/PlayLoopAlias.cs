using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayLoopAlias : MonoBehaviour
{
    [Aliase] [SerializeField] private int aliasToPlay;
    private AudioPlayer _audioPlayer;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            if (aliasToPlay != 0)
            {
                if(AudioManager.GetAlias(aliasToPlay, out Alias alias))
                    gameObject.name = $"Play Loop : {alias.name}";
            }
        #endif
    }

    private void Start()
    {
        PlayAlias();
    }

    public void PlayAlias()
    {
        transform.PlayLoopSound(aliasToPlay,ref _audioPlayer );
    }
}
