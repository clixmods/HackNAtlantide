using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CookieMove : MonoBehaviour
{
    [SerializeField] private float cookieSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var light = GetComponent<UniversalAdditionalLightData>();
        light.lightCookieOffset +=  Time.deltaTime * cookieSpeed * Vector2.one ;
    }
}
