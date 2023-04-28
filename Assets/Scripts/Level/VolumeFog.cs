using System.Collections;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Collider))]
public class VolumeFog : MonoBehaviour
{
    public delegate void VolumeFogEvent();
    private static VolumeFogEvent OnVolumeActive;
     [SerializeField] private VolumeFogSettings fogSetting = new VolumeFogSettings();
    private void Awake()
    {
        OnVolumeActive += () => StopCoroutine(Lerp());
    }
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            OnValidate();
        }
    }
    #endif
   

    private void OnValidate()
    {
        RenderSettings.fogDensity = fogSetting.fogDensity;
        RenderSettings.fogEndDistance = fogSetting.fogEndDistance;
        RenderSettings.fogStartDistance = fogSetting.fogStartDistance;
        RenderSettings.fogColor =  fogSetting.fogColor;
        RenderSettings.fog =  fogSetting.fog;
        RenderSettings.fogMode = fogSetting.fogMode;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnVolumeActive?.Invoke();
        StartCoroutine(Lerp());
        RenderSettings.fog =  fogSetting.fog;
        RenderSettings.fogMode = fogSetting.fogMode;
    }
    IEnumerator Lerp()
    {
        float timeElapsed = 0;
        var fogDensity = RenderSettings.fogDensity;
        var fogEndDistance = RenderSettings.fogEndDistance;
        var fogStartDistance = RenderSettings.fogStartDistance;
        var fogColor = RenderSettings.fogColor;
        while (timeElapsed < fogSetting.timeTransition)
        {
            var t = timeElapsed / fogSetting.timeTransition;
            RenderSettings.fogDensity = Mathf.Lerp(fogDensity, fogSetting.fogDensity, t);
            RenderSettings.fogEndDistance = Mathf.Lerp(fogEndDistance, fogSetting.fogEndDistance, t);
            RenderSettings.fogStartDistance = Mathf.Lerp(fogStartDistance, fogSetting.fogStartDistance, t);
            RenderSettings.fogColor = Color.Lerp(fogColor, fogSetting.fogColor, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }


}
