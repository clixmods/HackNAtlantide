using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy;

    // Update is called once per frame
    void Update()
    {
        _timeToDestroy -=Time.deltaTime;
        if( _timeToDestroy < 0 )
        {
            Destroy( this );
        }
    }
}
