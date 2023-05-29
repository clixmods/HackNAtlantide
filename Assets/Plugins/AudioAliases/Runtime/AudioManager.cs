using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


namespace AudioAliase
{
    public enum SoundType
    {
        Root,
        Start,
        End,
        Surface
    }

    public static class AudioManagerExtension 
    {
        public static void PlayLoopSound(this Transform transform, int aliaseName, ref AudioPlayer audioPlayerLoop)
        {
            AudioManager.PlayLoopSound(aliaseName, transform, ref audioPlayerLoop);
        }
        public static void PlayLoopSound(this GameObject gameObject, int aliaseName, ref AudioPlayer audioPlayerLoop)
        {
            AudioManager.PlayLoopSound(aliaseName, gameObject.transform, ref audioPlayerLoop);
        }
        public static Alias PlaySoundAtPosition(this GameObject gameObject, int aliaseName)
        {
            return AudioManager.PlaySoundAtPosition(aliaseName, gameObject.transform.position);
        }
        public static Alias PlaySoundAtPosition(this Transform transform, int aliaseName)
        {
            return AudioManager.PlaySoundAtPosition(aliaseName, transform.position);
        }
     
        
        
    }
    
    public partial class AudioManager : MonoBehaviour
    {
        #region Singleton

        private static AudioManager _instance;
        private static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    if(_instance == null)
                        _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
                }

                return _instance;
            }
            set => _instance = value;
        }
        

        #endregion

        public bool debugText = false;
        public static bool ShowDebugText => Instance.debugText;
        // public static Aliases[] aliasesArray
        // {
        //     get { return Instance._audioManagerData.aliases; }
        // }

        private static Dictionary<int, Alias> cachedDictionaryAliases;
        public static Dictionary<int, Alias> DictionaryAliases
        {
            get
            {
                if (cachedDictionaryAliases == null)
                {
                    cachedDictionaryAliases = Resources.Load<AudioManagerData>("AudioManager Data").DictionaryAliases;
                }

                return cachedDictionaryAliases;
            }
        }
        [SerializeField] private List<AudioPlayer> _audioSource;
        public const string AliasNameNull = "None";
        [SerializeField] private int audioSourcePoolSize = 128; // 32 is a good start
        private static Vector3 positionDefault = Vector3.zero;
        [Header("Debug")]
        private bool _isPaused;
        
        [HideInInspector] [SerializeReference] private AudioManagerData _audioManagerData;
        public bool IsPaused
        {
            set { _isPaused = value; }
        }
        
        private void InitAudioSources()
        {
            _audioSource = new List<AudioPlayer>();
            for (int i = 0; i < audioSourcePoolSize; i++)
            {
                GameObject newAudioSource = new GameObject("Audio Source " + i);
                newAudioSource.transform.SetParent(transform);
                AudioPlayer audioS = newAudioSource.AddComponent<AudioPlayer>();
                Instance._audioSource.Add(audioS);
                newAudioSource.SetActive(false);
            }
        }
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
            
            
            _audioManagerData = (AudioManagerData) Resources.Load("AudioManager Data") ;
            InitAudioSources();
        }
        // Update is called once per frame
        private void Update()
        {
            if (_isPaused)
            {
                PauseAllAudio();
            }
            else
            {
                UnPauseAllAudio();
            }
        }
        private void DisableInusedAudioSource()
        {
            foreach (AudioPlayer aS in _audioSource)
            {
                if (!aS.IsUsable)
                {
                    aS.gameObject.SetActive(false);
                }
            }
        }

        public static bool GetAlias(int guid, out Alias alias)
        {
            alias = null;
            if (guid == 0) 
                return false;

            if (DictionaryAliases.TryGetValue(guid, out alias))
            {
                if (alias != null && !alias.IsPlayable())
                {
                    return false;
                }

                if (alias != null && alias.audio.Length == 0)
                {
                    if(ShowDebugText)
                        Debug.LogError("[AudioManager] : Aliase: " + guid + " contains no sounds.");
                    return false;
                }

            }
            // for (int i = 0; i < aliasesArray.Length; i++)
            // {
            //     foreach (Alias tempalias in aliasesArray[i].aliases)
            //     {
            //         if (tempalias.GUID == guid)
            //         {
            //             alias = tempalias;
            //
            //         }
            //     }
            // }

         
            if (alias == null)
            {
                if(ShowDebugText)Debug.LogWarning("[AudioManager] : Aliase: " + guid + " not found.");
                return false;
            }

            return true;
        }
        private static AudioSource GetAudioSource()
        {
            foreach (AudioPlayer aS in Instance._audioSource)
            {
                if (!aS.Source.isPlaying)
                {
                    return aS.Source;
                }
            }
            return null;
        }
        public static bool GetAudioPlayer(out AudioPlayer audioPlayer)
        {
            audioPlayer = null;
            foreach (AudioPlayer aS in Instance._audioSource)
            {
                if (aS.IsUsable && !aS.IsReserved)
                {
                    audioPlayer = aS;
                    return true;
                }
            }
            if(ShowDebugText)Debug.LogWarning($"AudioManager : Limits exceded for _audioSource, maybe you need to increase your audioSourcePoolSize (Size = {Instance.audioSourcePoolSize})");
            return false;
        }
        public static void PauseAllAudio()
        {
            foreach (AudioPlayer aS in Instance._audioSource)
            {
                if (aS.Source.isPlaying)
                {
                    aS.Source.Pause();
                }
            }
        }
        public static void UnPauseAllAudio()
        {
            foreach (AudioPlayer aS in Instance._audioSource)
            {
                if (aS.Source != null)
                {
                    aS.Source.UnPause();
                }
             
            }
        }
        private static bool IsValidAliase()
        {

            return false;
        }
        private void AliaseIsValid()
        {
            
        }
        public static Alias PlaySoundAtPosition(int aliaseName, Vector3 position = default)
        {
            if(GetAlias(aliaseName, out Alias clip) && GetAudioPlayer(out AudioPlayer audioPlayer))
            {
                audioPlayer.Setup(clip, position);

                if (clip.isPlaceholder)
                {
                    if(ShowDebugText) Debug.LogWarning("Un son placeholder a été jouer, il faut le changer , nom de l'aliase " + aliaseName);
                }
                PlaySoundAtPosition(clip.Secondary, position);
                return clip;
            }

            return null;
        }
        public static Alias PlaySoundAtPosition(int aliaseName, AudioPlayer audioPlayer ,Vector3 position = default)
        {
            if(GetAlias(aliaseName, out Alias clip))
            {
                audioPlayer.Setup(clip , position);
                if (clip.isPlaceholder)
                {
                    if(ShowDebugText) Debug.LogWarning("Un son placeholder a été jouer, il faut le changer , nom de l'aliase " + aliaseName);
                }
                PlaySoundAtPosition(clip.Secondary, position);
                return clip;
            }
            return null;
        }
        /// <summary>
        /// Play a loop sound on a desired transform. The audioplayer can move with the transform
        /// </summary>
        /// <param guid="aliaseName"></param>
        /// <param guid="transformToTarget">Transform to follow</param>
        /// <param guid="audioPlayerLoop"> A ref to AudioPlayer, it can be used with the method StopLoopSound</param>
        public static void PlayLoopSound(int aliaseName, Transform transformToTarget, ref AudioPlayer audioPlayerLoop)
        {
            PlayLoopSound(aliaseName, transformToTarget.position, ref audioPlayerLoop);
            if (audioPlayerLoop != null)
            {
                audioPlayerLoop.SetTransformToFollow(transformToTarget);
            }
           
        }
        /// <summary>
        /// Play a loop sound at the desired position
        /// </summary>
        /// <param guid="aliaseName"></param>
        /// <param guid="position">The position of the loop sound</param>
        /// <param guid="audioPlayerLoop"> A ref to <see cref="AudioPlayer"/>, it can be used with the method StopLoopSound</param>
        public static void PlayLoopSound(int aliaseName, Vector3 position, ref AudioPlayer audioPlayerLoop )
        {
            // Check if the AudioPlayer is keep by the caller object is valid.
            if (audioPlayerLoop != null && !audioPlayerLoop.IsUsable)
            {
                if (ShowDebugText)
                {
                    Debug.Log($"[AudioManager] PlayLoop {aliaseName} already played");
                }
                return;
            }
          
            // Check if the alias is valid
            if (!GetAlias(aliaseName, out Alias alias))
            {
                return ;
            }
            // Get a random audio Player
            if (!GetAudioPlayer(out AudioPlayer audioPlayer))
            {
                if(ShowDebugText) Debug.LogWarning($"AudioManager :green; ► Limits exceded for _audioSource, maybe you need to increase your audioSourcePoolSize (Size = {Instance.audioSourcePoolSize})");
                return ;
            }
            audioPlayer.Setup(alias , position);
            if (alias.isPlaceholder)
            {
                if(ShowDebugText) Debug.LogWarning("[AudioManager] Placeholder sound was played, guid " + aliaseName);
            }
            PlaySoundAtPosition(alias.Secondary, position);
            audioPlayerLoop = audioPlayer;
        }
        public static void StopLoopSound(ref AudioPlayer audioPlayer, StopLoopBehavior stopLoopBehavior = StopLoopBehavior.FinishCurrentPlay)
        {
            if (audioPlayer != null)
            {
                audioPlayer.StopSound(stopLoopBehavior);
            }

            audioPlayer = null;
        }
    }

    public enum StopLoopBehavior
    {
        Direct,
        FinishCurrentPlay
    }
    
    public class InvalidAliasesException : Exception {
        public InvalidAliasesException() : base() { }

        public InvalidAliasesException(string message) : base(message) { }

        public InvalidAliasesException(string message, Exception innerException) : base(message, innerException) { }
    }
}