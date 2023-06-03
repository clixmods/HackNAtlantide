using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RumblerDataPulse))]
public class RumblerDataPulseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (RumblerDataPulse)target;
        GUI.enabled = EditorApplication.isPlaying;
        {
            if (GUILayout.Button("Vibrate", GUILayout.Height(40), GUILayout.Width(80)))
            {
                script.Rumble();
            }
        }

    }
}
