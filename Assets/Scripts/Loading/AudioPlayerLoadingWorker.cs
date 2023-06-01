using System;
using AudioAliase;
using UnityEngine;

public class AudioPlayerLoadingWorker : LoadingWorkerBehaviour
{
    private bool _workIsDone;

    public override bool WorkIsDone
    {
        get => _workIsDone;
        set => _workIsDone = value;
    }

    private AudioPlayer _audioPlayer;

    protected override void Awake()
    {
        base.Awake();
        _audioPlayer = GetComponent<AudioPlayer>();
        _audioPlayer.OnAudioEnable += AudioEnable;
        _audioPlayer.OnAudioDisable += AudioDisable;
    }

    private void AudioDisable()
    {
        WorkIsDone = true;
    }

    private void AudioEnable()
    {
        WorkIsDone = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _audioPlayer.OnAudioEnable -= AudioEnable;
        _audioPlayer.OnAudioDisable -= AudioDisable;
        ;
    }
}