using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    public UnityEvent OnVideoStart;
    public UnityEvent OnVideoStop;
    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        enabled = false;
    }

    // Start is called before the first frame update
    public void StartVideo()
    {
        _videoPlayer.Play();
        OnVideoStart?.Invoke();
        enabled = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_videoPlayer.isPlaying)
        {
            OnVideoStop?.Invoke();
            enabled = false;
        }
    }
}
