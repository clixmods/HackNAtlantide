using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessWeightTransition : MonoBehaviour
{
    private Volume _volume;

    private float _weightTarget;
    private float _weightPrevious;
    [SerializeField] private float timeTransition = 3;

    public void SetWeightVolume(float value)
    {
        if(Math.Abs(value - _volume.weight) < 0.05f) 
            return;
        StopCoroutine(WeightTransition());
        _weightPrevious = _volume.weight;
        _weightTarget = value;
        StartCoroutine(WeightTransition());
    }
    
    IEnumerator WeightTransition()
    {
        float timeElapsed = 0;
        while (timeElapsed < timeTransition )
        {
            var t = timeElapsed / timeTransition;
    
            _volume.weight = Mathf.Clamp(Mathf.Lerp(_weightPrevious, _weightTarget, t) ,0,1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
    }
    // Update is called once per frame
    void Update()
    {
        _volume.weight = Mathf.Clamp(Mathf.Lerp(_volume.weight, _weightTarget, Time.unscaledDeltaTime * timeTransition),0,1);
    }
}
