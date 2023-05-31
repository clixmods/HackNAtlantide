using System;
using AudioAliase;
using UnityEngine;
using UnityEngine.VFX;

public class AliasVisualEffectListener : MonoBehaviour
{
    private VisualEffect _visualEffect;
        [SerializeField] private Alias alias;
        private bool _aliasIsPlayed;
        private AudioPlayer _audioPlayer;
    
        private void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
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
                if (_visualEffect.HasAnySystemAwake())
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
                if ( _visualEffect.HasAnySystemAwake())
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