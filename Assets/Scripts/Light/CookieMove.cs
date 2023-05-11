using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CookieMove : MonoBehaviour
{
    [SerializeField] private float cookieSpeed = 1;
    // Update is called once per frame
    void Update()
    {
        var light = GetComponent<UniversalAdditionalLightData>();
        light.lightCookieOffset +=  Time.deltaTime * cookieSpeed * Vector2.one ;
        if (light.lightCookieOffset.x >= light.lightCookieSize.x)
        {
            light.lightCookieOffset.Set(0, light.lightCookieOffset.y );
        }

        if (light.lightCookieOffset.y >= light.lightCookieSize.y)
        {
            light.lightCookieOffset.Set(light.lightCookieOffset.x, 0 );
        }
    }
}
