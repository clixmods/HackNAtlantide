using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event listener to attach methods directly with the inspector
/// </summary>
public class EventsListenerIDamageable : MonoBehaviour
{
    public UnityEvent OnDamage;
    public UnityEvent OnDeath;
    // Start is called before the first frame update
    void Awake()
    {
        var idamageable = GetComponent<IDamageable>();
        idamageable.OnDamage += IdamageableOnDamage;
        idamageable.OnDeath += IdamageableOnDeath;
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
