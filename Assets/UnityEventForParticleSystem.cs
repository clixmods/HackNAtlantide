using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;

public class UnityEventForParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField] private Alias alias;
    private bool _aliasIsPlayed;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
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
