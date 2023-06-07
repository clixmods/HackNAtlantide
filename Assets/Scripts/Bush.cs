using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Bush : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleBush;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 14)
        {
            _particleBush.Play();
        }
    }
}
