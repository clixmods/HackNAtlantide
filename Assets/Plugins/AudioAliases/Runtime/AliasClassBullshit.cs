using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace AudioAliase
{

    [Serializable]
    public struct SurfaceAlias
    {
        public string surfaceName;
        public AliasSurface alias;
    }
    [System.Serializable]
    public class AliasClassBullshit
    {
        public string name;
        public string description;
        public string Tag;
        [Tooltip("Yo")]
        public AudioMixerGroup MixerGroup;
        public AudioClip[] audio;

        public SoundType soundType = SoundType.Root;

        [Tooltip("Take a random clip, each time the aliase is played")]
        public bool randomizeClips;
        [Tooltip("Index for the randomizeClips")]
        int _indexRandomize = 0;

        [Tooltip("Secondary aliase played with the primary aliase")]
        public int Secondary;

        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        [Range(0, 256)]
        public float priority;
        // TODO : Do a random range
        [Range(0, 1)]
        public float volume = 0.8f; // Obsoletes
        [Tooltip("Min and Max volume range")]
        public float minVolume = 0.8f;
        public float maxVolume = 0.8f;
        [Tooltip("How many time a alias can be played in the same time ?")]
        public int limitCount;
        /* Futur implementation
         * public float FadeIn;
         * public float FadeOut;
         * public float Probability;
         * public AliasClassBullshit StopAlias
         */
        public bool isLooping;
        [Tooltip("AliasClassBullshit played in the start of a looped alias")]
        public int startAliase;
        [Tooltip("AliasClassBullshit played when a looped alias is stopped")]
        public int endAliase;
        public bool UseDelayLoop;
        public float minDelayLoop = 0;
        public float maxDelayLoop = 0;

        [Range(-3, 3)]
        public float minPitch = 1f;
        [Range(-3, 3)]
        public float maxPitch = 1.01f;
        [Range(-1, 1)]
        public float stereoPan = 0;
        [Range(0, 1)]
        public float spatialBlend = 0;

        [Range(0, 1.1f)]
        public float reverbZoneMix = 1;
        [Header("3D Sound Settings")]
        [Range(0, 5)]
        public float dopplerLevel = 1;
        [Range(0, 360)]
        public float Spread = 1;
        [Range(0, 10000)]
        public float MinDistance = 1;
        [Range(0, 10000)]
        public float MaxDistance = 500;
        public AudioRolloffMode CurveType = AudioRolloffMode.Logarithmic;
        public AnimationCurve distanceCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });
        
        [Header("Subtitle")]
        public string Text;
        public float customDuration;

        public bool isInit;
        public bool isPlaceholder = true;
        [Header("Instance")] 
        public List<AudioPlayer> audioPlayers;
        [Tooltip("The alias will detect surface from material to play the adequat alias [XMaterials plugin required]")]
        public bool UseSurfaceDetection;
        public SurfaceAlias[] surfacesAlias;
        public Dictionary<string, Alias> dictSurfacesAlias ;
        [SerializeField] private int guid = AliasUtility.GenerateID();
        public int GUID => guid; 
        public AudioClip Audio
        {
            get
            {
                if (randomizeClips)
                {
                    _indexRandomize = Random.Range(0, audio.Length);
                    return audio[_indexRandomize];
                }
                return audio[0];
            }
        }

        public float DelayLoop
        {
            get
            {
                float delayValue = Random.Range(  minDelayLoop, maxDelayLoop);
                return delayValue;
            }
        }
        public bool HasDelayLoop
        {
            get
            {
                return minDelayLoop == 0 && maxDelayLoop == 0;
            }
        }

        // Give default value for a new aliase
        // We override the default constructor, because Unity doesnt give default value when we initialize variable
        // will be fix by unity in the future...
        public AliasClassBullshit()
        {
            name = "newAliase";
            volume = 0.8f;
            minVolume = 0.8f;
            maxVolume = 0.8f;
            minPitch = 1f;
            maxPitch = 1.01f;
            reverbZoneMix = 1;
            dopplerLevel = 1;
            Spread = 1;
            MinDistance = 1;
            MaxDistance = 500;
        }

        public bool IsPlayable()
        {
            if (limitCount == 0)
                return true;

            return limitCount > audioPlayers.Count;
        }
        
    }

}