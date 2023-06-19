using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityLerp : MonoBehaviour
{
    [SerializeField] private float lerpDuration = 1;
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    public void SetIntensityTo(float intensity)
    {
        StartCoroutine(ChangeIntensity(intensity));
    }

    IEnumerator ChangeIntensity(float targetIntensity)
    {
        float startIntensity = _light.intensity;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / lerpDuration ;
            _light.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }
        yield return null;
    }
}
