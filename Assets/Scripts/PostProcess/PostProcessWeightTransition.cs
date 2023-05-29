using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessWeightTransition : MonoBehaviour
{
    private Volume _volume;
    private bool _isTransitioning;
    private float _weightTarget;
    private float _weightPrevious;
    private float _timeElapsed;
    [SerializeField] private float timeTransition = 3;
    //[SerializeField] private bool ignoreTimescale = true;
    void Awake()
    {
        _volume = GetComponent<Volume>();
    }
    public void SetWeightVolume(float value)
    {
        if(Math.Abs(value - _volume.weight) < 0.05f) 
            return;
        //StopCoroutine(WeightTransition());
        _weightPrevious = _volume.weight;
        _weightTarget = value;
         _timeElapsed = 0;
        if( !_isTransitioning)
            StartCoroutine(WeightTransition());
    }
    public void SetWeightVolume(float value, float timeToTransit)
    {
        timeTransition = timeToTransit;
        SetWeightVolume(value);
    }
    IEnumerator WeightTransition()
    {
        _isTransitioning = true;
        
        while (_timeElapsed < timeTransition )
        {
            float t = _timeElapsed / timeTransition;
            _volume.weight = Mathf.Clamp(Mathf.Lerp(_weightPrevious, _weightTarget, t) ,0,1);
            _timeElapsed += Time.deltaTime;
            yield return null;
        }
        _isTransitioning = false;
    }

    

}
