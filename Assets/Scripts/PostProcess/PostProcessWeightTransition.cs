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
    [Header("Temporary Settings")]
    [SerializeField] private bool isTemporary;
    [SerializeField] private float duration = 2f;
    [Header("Flick Settings")]
    [SerializeField] private bool flickering = false;
    [Range(0,1f)]
    [SerializeField] private float minWeight = 0;
    [Range(0,1f)]
    [SerializeField] private float maxWeight = 1;
    private bool _isActive = false;
    [SerializeField] private float speedMultiplier = 1f;

    //[SerializeField] private bool ignoreTimescale = true;
    void Awake()
    {
        _volume = GetComponent<Volume>();
    }
    public void SetWeightVolume(float value)
    {
        if (_volume == null)
        {
            _volume = GetComponent<Volume>();
        }
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

    public void SetWeightVolumeDirect(float value)
    {
        _volume.weight = value;
    }
    IEnumerator WeightTransition()
    {
        _isTransitioning = true;
        
        while (_timeElapsed < timeTransition )
        {
            float t = _timeElapsed / timeTransition;
            _volume.weight = Mathf.Clamp(Mathf.Lerp(_weightPrevious, _weightTarget, t) ,0,1);
            _timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_weightTarget > 0)
        {
            _isActive = true;
            if(isTemporary)
                StartCoroutine(ActiveInXSeconds());
        }
        else
        {
            _isActive = false;
        }
        _volume.weight = _weightTarget;
        _isTransitioning = false;
    }

    private IEnumerator ActiveInXSeconds()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(duration);
        SetWeightVolume(0);
    }

    private void Update()
    {
        if(_isActive && flickering)
            _volume.weight = Mathf.Lerp(minWeight, maxWeight, (Mathf.Sin(Time.time*speedMultiplier) + 1f)/2f );
    }
}
