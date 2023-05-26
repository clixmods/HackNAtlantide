using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerGroupManipulator : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    public void SetVolume(float value)
    {
        audioMixerGroup.audioMixer.SetFloat()
    }
}
