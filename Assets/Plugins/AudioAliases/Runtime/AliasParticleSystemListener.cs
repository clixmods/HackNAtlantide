using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;

public class AliasParticleSystemListener : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField] private Alias alias;
    private bool _aliasIsPlayed;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        if (alias.isLooping)
        {
            AudioManager.StopLoopSound(ref _audioPlayer, StopLoopBehavior.Direct);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alias.isLooping)
        {
            if (_particleSystem.isPlaying)
            {
                if (!_aliasIsPlayed)
                {
                    transform.PlayLoopSound(alias, ref _audioPlayer);
                    _aliasIsPlayed = true;
                }
            }
            else
            {
                AudioManager.StopLoopSound(ref _audioPlayer, StopLoopBehavior.Direct);
                _aliasIsPlayed = false;
            }
        }
        else
        {
            if ( _particleSystem.isPlaying)
            {
                if (!_aliasIsPlayed)
                {
                    transform.PlaySoundAtPosition(alias);
                    _aliasIsPlayed = true;
                }
                
            }
            else
            {
            
                _aliasIsPlayed = false;
            }
        }
       
    }
}
