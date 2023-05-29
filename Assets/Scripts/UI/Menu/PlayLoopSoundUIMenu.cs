using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[AddComponentMenu("#Survival Game/UI/UI Menu PlayLoop Sound alias")]
public class PlayLoopSoundUIMenu : MonoBehaviour
{
    [Header("Aliases")] 
    // private float _currentDelay;
    // [SerializeField] private float delayBeforeStartLoop;
    [SerializeField] private Alias MusicLoop;

    private AudioPlayer _audioPlayer;
    private void Awake()
    {
        GetComponent<UIMenu>().EventOnOpenMenu.AddListener(PlayMusic) ;
        GetComponent<UIMenu>().EventOnCloseMenu.AddListener(StopMusic) ;
    }

    private void StopMusic()
    {
        AudioManager.StopLoopSound(ref _audioPlayer);
    }

    private void PlayMusic()
    {
        transform.PlayLoopSound(MusicLoop ,ref _audioPlayer);
    }
}