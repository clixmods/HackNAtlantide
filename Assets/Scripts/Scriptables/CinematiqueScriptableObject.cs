using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Data/CinematiqueData")]
public class CinematiqueScriptableObject : ScriptableObject
{
    [SerializeField] TimelineAsset _timeline;
    [SerializeField] bool _isInputActive;
    [SerializeField] bool _skipable;
    [SerializeField] Action onCinematiqueEnd;
    public void LaunchCinematique()
    {
        //_timeline.play
        FindObjectOfType<CinematiqueStateBehaviour>().gameObject.SetActive(true);
        //DisactiveGameplayInputs

    }
    public void Skip()
    {
        if(_skipable)
        {
            //_timeline.Stop
            //fadeBlackScreen
            onCinematiqueEnd?.Invoke();
        }
    }
}
