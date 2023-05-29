using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraShakeScriptableObject))]
public class CameraShakeScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (CameraShakeScriptableObject)target;
        GUI.enabled = EditorApplication.isPlaying;
        {
            if (GUILayout.Button("Shake", GUILayout.Height(40), GUILayout.Width(80)))
            {
                script.Shake();
            }
        }

    }

}
