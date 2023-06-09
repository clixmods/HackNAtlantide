﻿using System;
using AudioAliase;
using UnityEngine;


    public class AddWorkerAudioPlayerHandler : MonoBehaviour
    {
        
        private void OnEnable()
        {
            var audioPlayers = FindObjectsOfType<AudioPlayer>();
            for (int i = 0; i < audioPlayers.Length; i++)
            {
                if (audioPlayers[i].TryGetComponent<AudioPlayerLoadingWorker>(out var audioPlayerLoadingWorker))
                    continue;    
            
                audioPlayers[i].gameObject.AddComponent<AudioPlayerLoadingWorker>();
            }
            
        }
    }
