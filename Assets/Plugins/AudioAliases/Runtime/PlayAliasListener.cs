using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayAliasListener : MonoBehaviour
{
     [SerializeField] private Alias aliasToPlay;

     private void OnValidate()
     {
#if UNITY_EDITOR
         if (aliasToPlay != null)
         {
             gameObject.name = $"Play  : {aliasToPlay.name}";
         }
#endif
     }
    private void OnEnable()
    {
        PlayAlias();
    }

    private void OnDisable()
    {
        
    }

    public void PlayAlias()
    {
        transform.PlaySoundAtPosition(aliasToPlay);
    }
}
