using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[AddComponentMenu("#Survival Game/UI/UI Menu PlaySound alias")]
public class PlaySoundUIMenu : MonoBehaviour
{
    [Header("Aliases")]
    [SerializeField]
    private Alias MusicLoop;

    private void Awake()
    {
        GetComponent<UIMenu>().EventOnOpenMenu.AddListener(PlayMusic) ;
    }

    private void PlayMusic()
    {
        transform.PlaySoundAtPosition(MusicLoop);
    }
}