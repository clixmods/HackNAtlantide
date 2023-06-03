using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesWatcher : MonoBehaviour
{
    [SerializeField] private List<Character> _charactersToWatch;
    private List<EnemyWakeUpBehaviour> _enemyWakeUpBehaviours;
    public UnityEvent NoCharactersToWatch;
    [SerializeField] private bool GetChildEnnemies;
    [SerializeField] private bool DisableWakeUpBehaviourInStart;
    private void OnValidate()
    {
        if (GetChildEnnemies)
        {
            _charactersToWatch = GetComponentsInChildren<Character>().ToList();
            GetChildEnnemies = false;
        }
    }

    private void Awake()
    {
        _enemyWakeUpBehaviours = new List<EnemyWakeUpBehaviour>();
        for (int i = 0; i < _charactersToWatch.Count; i++)
        {
            if (_charactersToWatch[i].TryGetComponent<EnemyWakeUpBehaviour>(out var enemyWakeUpBehaviour))
            {
                _enemyWakeUpBehaviours.Add(enemyWakeUpBehaviour);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < _charactersToWatch.Count; i++)
        {
            _charactersToWatch[i].OnDeath += OnDeath;
        }
        if (DisableWakeUpBehaviourInStart)
        {
            for (int i = 0; i < _enemyWakeUpBehaviours.Count; i++)
            {
                _enemyWakeUpBehaviours[i].enabled = false;
            }
        }
    }

    public void EnableWakeUpBehaviourInWatchedEnemies()
    {
        for (int i = 0; i < _enemyWakeUpBehaviours.Count; i++)
        {
            if (_enemyWakeUpBehaviours[i] != null)
            {
                _enemyWakeUpBehaviours[i].enabled = true;
            }
            
        }
    }

    private void OnDeath(object sender, EventArgs e)
    {
        if (_charactersToWatch.Contains((Character) sender))
        {
            _charactersToWatch.Remove((Character)sender);
        }
        if (_charactersToWatch.Count <= 0)
        {
            enabled = false;
            NoCharactersToWatch?.Invoke();
            
        }
    }

    public void KillAllEnemiesWatchedWithoutSendWatcherEvent()
    {
        if (!enabled) return;
        
        if (_charactersToWatch.Count == 0)
        {
            return;
        }
            
        for (int i = 0; i < _charactersToWatch.Count; i++)
        {
            if (_charactersToWatch[i] != null)
            {
                _charactersToWatch[i].OnDeath -= OnDeath;
                _charactersToWatch[i].Dead();
            }
        }
    }
    public void KillAllEnemiesWatched()
    {
        if (!enabled) return;
        
        if (_charactersToWatch.Count == 0)
        {
            return;
        }
        for (int i = 0; i < _charactersToWatch.Count; i++)
        {
            if (_charactersToWatch[i] != null)
            {
                _charactersToWatch[i].Dead();
            }
        }
    }
    
}
