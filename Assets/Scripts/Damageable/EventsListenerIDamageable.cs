using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event listener to attach methods directly with the inspector
/// </summary>
public class EventsListenerIDamageable : MonoBehaviour
{
    private IDamageable _idamageable;
    public UnityEvent OnDamage;
    public UnityEvent OnDeath;
    // Start is called before the first frame update
    void Awake()
    {
        _idamageable = GetComponent<IDamageable>();
        _idamageable.OnDamage += IdamageableOnDamage;
        _idamageable.OnDeath += IdamageableOnDeath;
    }

    private void OnDestroy()
    {
        _idamageable.OnDamage -= IdamageableOnDamage;
        _idamageable.OnDeath -= IdamageableOnDeath;
    }

    private void IdamageableOnDeath(object sender, EventArgs e)
    {
        OnDeath?.Invoke();
    }
    private void IdamageableOnDamage(object sender, EventArgs e)
    {
        OnDamage?.Invoke();
    }
}
