using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace AudioAliase
{
    public enum CurrentlyPlaying
    {
        /// <summary>
        /// The start sound is defined
        /// </summary>
        Start,
        Base,
        End,
    }

    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(GetSurfaceType))]
    public class AudioPlayer : MonoBehaviour
    {
        public Queue<Alias> _clips = new();
        
        [SerializeField] private Alias lastAliasPlayed;

        [SerializeField] private bool forceStop;

        private bool _isStopping;
        private StopLoopBehavior _stopLoopBehavior;
        [SerializeField] private Alias OnStartAliaseToPlay;
        private float _delayLoop = 0;

        #region Private Variable

        private bool _startWasPlayed;

        // TODO : Not good to use aliase in properties because it will be copied (serialize shit), we need to use simply string
        private Alias _nextSound;
        private Transform _transformToFollow;
        private float _timePlayed = 0;
        private CurrentlyPlaying _state = CurrentlyPlaying.Start;

        #endregion

        public AudioSource Source { get; private set; }

        /// <summary>
        /// AudioPlayer is available ? 
        /// </summary>
        public bool IsUsable => _clips.Count == 0 && !Source.isPlaying && !gameObject.activeSelf;

        public bool IsFollowingTransform => _transformToFollow != null;

        [Tooltip("Specify if the audioplayer is reserved by another object, the pool will not use it")] [SerializeField]
        private bool isReserved;

        /// <summary>
        /// Specify if the audioplayer is reserved by another object, the pool will not use it
        /// </summary>
        public bool IsReserved
        {
            get => isReserved;
            set => isReserved = value;
        }

        public void SetTransformToFollow(Transform transformTarget)
        {
            _transformToFollow = transformTarget;
        }

        #region MonoBehaviour

        private void Awake()
        {
            Source = transform.GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (OnStartAliaseToPlay != null)
            {
                Play(OnStartAliaseToPlay);
            }
        }

        IEnumerator StopAliasVolume()
        {
            float timeElapsed = 0;
            float currentVolume = Source.volume;
            while (timeElapsed < lastAliasPlayed.CloseFadeInSeconds )
            {
                float t = timeElapsed / lastAliasPlayed.CloseFadeInSeconds;
                Source.volume = Mathf.Clamp(Mathf.Lerp(currentVolume, 0, t) ,0,1);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
            Reset();
        }
        IEnumerator StartAliasVolume()
        {
            float timeElapsed = 0;
            float targetVolume = lastAliasPlayed.volume;
            while (timeElapsed < lastAliasPlayed.OpenFadeInSeconds )
            {
                float t = timeElapsed / lastAliasPlayed.CloseFadeInSeconds;
                Source.volume = Mathf.Clamp(Mathf.Lerp(0, targetVolume, t) ,0,1);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        // Update is called once per frame
        private void Update()
        {
            if (lastAliasPlayed == null) 
                gameObject.SetActive(false);
            
            FollowTransform();
            if (Source.clip == null)
            {
                StartCoroutine(StopAliasVolume());
                return;
            }
                
            // Audio play have finish the play
            if (_timePlayed + lastAliasPlayed.CloseFadeInSeconds >= (Source.clip.length * Source.pitch) + _delayLoop)
            {
                if (_isStopping)
                {
                    StartCoroutine(StopAliasVolume());
                   
                    return;
                }

                if (lastAliasPlayed.isLooping)
                {
                    SetupAudioSource(lastAliasPlayed);
                    if (_delayLoop != 0)
                        Source.Play();

                    _timePlayed = 0;
                }
                else // End of the sound
                {
                    switch (_state)
                    {
                        case CurrentlyPlaying.Start:
                            //_state = CurrentlyPlaying.Base;
                            SetupAudioSource(_nextSound);
                            //Source.clip = _nextSound.Audio; 
                            Source.Play();
                            break;
                        case CurrentlyPlaying.Base:
                            StopSound(_stopLoopBehavior);
                            break;
                        case CurrentlyPlaying.End:
                        default:
                            StartCoroutine(StopAliasVolume());
                            break;
                    }

                    _timePlayed = 0;
                    _state++;
                }
            }
            else
            {
                _timePlayed += Time.unscaledDeltaTime;
            }
        }

        private void Reset()
        {
            Source.Stop();
            lastAliasPlayed = null;
            _transformToFollow = null;
            _state = CurrentlyPlaying.Start;
            _timePlayed = 0;
            _nextSound = null;
        }

        #endregion

        private void Play(Alias aliasToPlay)
        {
            // If a start aliase is available, we need to play it before the base aliase
            if (_state == CurrentlyPlaying.Start && aliasToPlay.startAliase != null)
            {
                SetupAudioSource(aliasToPlay.startAliase);
                //Source.clip = startLoop.Audio;
                Source.Play();
                _nextSound = aliasToPlay;
                return;
            }

            _state = CurrentlyPlaying.Base; // Sinon ca fait le bug du next sound pas def
            //Setup the base aliase
            SetupAudioSource(aliasToPlay);
            //Source.clip = aliasToPlay.Audio;
            Source.Play();
        }

        private void Play(int onStartAliasToPlay)
        {
            AudioManager.GetAlias(onStartAliasToPlay, out var aliase);
            Play(aliase);
        }

        public void Setup(Alias aliasToPlay, Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            Reset();
            Play(aliasToPlay);
        }

        private void FollowTransform()
        {
            if (IsFollowingTransform)
            {
                transform.position = _transformToFollow.position;
            }
        }

        public void StopSound(StopLoopBehavior stopLoopBehavior)
        {
            if (lastAliasPlayed != null && lastAliasPlayed.audioPlayers.Contains(this))
                lastAliasPlayed.audioPlayers.Remove(this);

            _stopLoopBehavior = stopLoopBehavior;
            if (_state == CurrentlyPlaying.Start)
            {
                switch (stopLoopBehavior)
                {
                    case StopLoopBehavior.Direct:
                        StartCoroutine(StopAliasVolume());
                        break;
                    case StopLoopBehavior.FinishCurrentPlay:
                        _isStopping = true;
                        break;
                    default:
                        StartCoroutine(StopAliasVolume());
                        break;
                }

                return;
            }

            if (_state == CurrentlyPlaying.Base
                && lastAliasPlayed.endAliase != null)
            {
                SetupAudioSource(lastAliasPlayed.endAliase);
                //Source.clip = stopLoop.Audio;
                Source.Play();
            }
            else
            {
                switch (stopLoopBehavior)
                {
                    case StopLoopBehavior.Direct:
                        StartCoroutine(StopAliasVolume());
                        break;
                    case StopLoopBehavior.FinishCurrentPlay:
                        _isStopping = true;
                        break;
                    default:
                        StartCoroutine(StopAliasVolume());
                        break;
                }
            }

            _state++;
        }

        public void SetupAudioSource(Alias alias)
        {
            if (alias == null)
            {
                if (AudioManager.ShowDebugText) 
                    Debug.LogError("What the fuck ?");
            }

            if (lastAliasPlayed != null && lastAliasPlayed.audioPlayers.Contains(this))
                lastAliasPlayed.audioPlayers.Remove(this);

            if (!alias.audioPlayers.Contains(this))
                alias.audioPlayers.Add(this);

            _timePlayed = 0;
            _isStopping = false;

            lastAliasPlayed = alias;
            var audiosource = Source;
            audiosource.volume = lastAliasPlayed.volume;
            StartCoroutine(StartAliasVolume());
            // Check if Looping
            if (alias.isLooping)
            {
                _delayLoop = alias.DelayLoop;
                if (alias.HasDelayLoop)
                {
                    Source.loop = true;
                }
            }
            else
            {
                Source.loop = false;
                _delayLoop = 0;
            }
            // Setup audio clip
            if (alias.UseSurfaceDetection)
            {
                var surfaceName = GetComponent<GetSurfaceType>().SphereCast();
                if (surfaceName != null && alias.dictSurfacesAlias.TryGetValue(surfaceName, out Alias value))
                {
                    Debug.Log(surfaceName);
                    Play(value);
                    return;
                }

                Source.clip = alias.Audio;
            }
            else
            {
                Source.clip = alias.Audio;
            }


            audiosource.pitch = Random.Range(alias.minPitch, alias.maxPitch);
            audiosource.spatialBlend = alias.spatialBlend;
            if (alias.MixerGroup != null)
                audiosource.outputAudioMixerGroup = alias.MixerGroup;

            switch (alias.CurveType)
            {
                case AudioRolloffMode.Logarithmic:
                case AudioRolloffMode.Linear:
                    audiosource.rolloffMode = alias.CurveType;
                    break;
                case AudioRolloffMode.Custom:
                    audiosource.rolloffMode = alias.CurveType;
                    audiosource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, alias.distanceCurve);
                    break;
            }
        }
    }
}