using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalLifeTime : MonoBehaviour
{
    private DecalProjector _decalProjector;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float timeToDisappear = 2f;
    private void Awake()
    {
        _decalProjector = GetComponent<DecalProjector>();
        StartCoroutine(LifeTimeCoroutine());
    }

    IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        float t = 0;
        while (t < timeToDisappear)
        {
            t += Time.deltaTime;
            _decalProjector.fadeFactor = 1f - t/timeToDisappear;
            yield return null;
        }
        Destroy(gameObject);
    }
    
}
