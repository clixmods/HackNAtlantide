using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy = 3f;
    float _time;
    [SerializeField] bool _randomValue;
    [SerializeField] bool _shrink = true;
    Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
        _time = _timeToDestroy;
        if(_randomValue)
        {
            _timeToDestroy = (Random.value+0.5f) *_timeToDestroy;
        }
    }
    // Update is called once per frame
    void Update()
    {
        _timeToDestroy -= Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.zero,initialScale,_timeToDestroy / _time);
        if( _timeToDestroy < 0 )
        {
            Destroy( gameObject );
        }
    }
}
