using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesWatcher : MonoBehaviour
{
    [SerializeField] private List<Character> _charactersToWatch;
    public UnityEvent NoCharactersToWatch;
    private void Awake()
    {
        for (int i = 0; i < _charactersToWatch.Count; i++)
        {
            _charactersToWatch[i].OnDeath += OnDeath;
        }
    }

    private void OnDeath(object sender, EventArgs e)
    {
        _charactersToWatch.Remove((Character)sender);
        if (_charactersToWatch.Count <= 0)
        {
            NoCharactersToWatch?.Invoke();
            this.enabled = false;
        }
    }
}
