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
    }
}
