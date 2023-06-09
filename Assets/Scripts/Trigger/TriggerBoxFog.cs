using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class TriggerBoxFog : TriggerBox
{
    public delegate void VolumeFogEvent();
    private static VolumeFogEvent OnVolumeActive;
    [SerializeField] private VolumeFogSettings fogSetting = new VolumeFogSettings();
    [SerializeField] private bool previewFog;
    protected override void Awake()
    {
        base.Awake();
        OnVolumeActive += StopLerpOnEvent;
    }

    private void OnDestroy()
    {
        OnVolumeActive -= StopLerpOnEvent;
    }

    private void StopLerpOnEvent()
    {
        StopCoroutine(Lerp());
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
    protected override void OnValidate()
    {
        base.OnValidate();
        if (previewFog)
        {
            RenderSettings.fogDensity = fogSetting.fogDensity;
            RenderSettings.fogEndDistance = fogSetting.fogEndDistance;
            RenderSettings.fogStartDistance = fogSetting.fogStartDistance;
            RenderSettings.fogColor =  fogSetting.fogColor;
            RenderSettings.fog =  fogSetting.fog;
            RenderSettings.fogMode = fogSetting.fogMode;
        }
        
    }

    protected override void TriggerEnter(Collider other)
    {
        base.TriggerEnter( other);
        ApplyFog();
    }

    public void ApplyFog()
    {
        OnVolumeActive?.Invoke();
        StartCoroutine(Lerp());
        RenderSettings.fog =  fogSetting.fog;
        RenderSettings.fogMode = fogSetting.fogMode;
        Debug.Log("Fog Changed !");
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
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Custom/Trigger Box Fog", false, 1)]
    static void CreateTriggerBox(MenuCommand menuCommand)
    {
        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            // Create a custom game object
            GameObject triggerGameObject = new GameObject("Box Trigger Fog");
            var raycastHits = Physics.RaycastAll(view.camera.transform.position, view.camera.transform.forward);
            if (raycastHits.Length > 0)
            {
                triggerGameObject.transform.position = raycastHits[^1].point;
            }
            GameObjectUtility.SetParentAndAlign(triggerGameObject, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(triggerGameObject, "Create " + triggerGameObject.name);
            Selection.activeObject = triggerGameObject;
            // Create TriggerBox
            var triggerBox = triggerGameObject.AddComponent<TriggerBoxFog>();
        }
                 
    }
#endif
}
