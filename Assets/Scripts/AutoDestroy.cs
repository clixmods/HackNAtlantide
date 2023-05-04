using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy;
    [SerializeField] bool _randomValue;

    private void Awake()
    {
        if(_randomValue)
        {
            _timeToDestroy = (Random.value+0.5f) *_timeToDestroy;
        }
    }
    // Update is called once per frame
    void Update()
    {
        _timeToDestroy -= Time.deltaTime;
        if( _timeToDestroy < 0 )
        {
            Destroy( gameObject );
        }
    }
}
