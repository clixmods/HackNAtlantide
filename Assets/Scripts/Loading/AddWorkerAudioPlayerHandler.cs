using System;
using AudioAliase;
using Unity.VisualScripting;
using UnityEngine;

namespace Loading
{
    public class AddWorkerAudioPlayerHandler : MonoBehaviour
    {
        
        private void OnEnable()
        {
            var audioPlayers = FindObjectsOfType<AudioPlayer>();
            for (int i = 0; i < audioPlayers.Length; i++)
            {
                if (audioPlayers[i].TryGetComponent<AudioPlayerLoadingWorker>(out var audioPlayerLoadingWorker))
                    continue;    
            
                audioPlayers[i].AddComponent<AudioPlayerLoadingWorker>();
            }
            
        }
    }
}