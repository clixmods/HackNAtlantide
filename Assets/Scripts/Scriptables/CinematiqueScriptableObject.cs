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
    [SerializeField] bool skipable;
    [SerializeField] Action onCinematiqueEnd;
}
