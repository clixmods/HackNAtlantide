using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(RumblerDataConstant))]
public class RumblerDataConstantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (RumblerDataConstant)target;
        GUI.enabled = EditorApplication.isPlaying;
        {
            if (GUILayout.Button("Vibrate", GUILayout.Height(40), GUILayout.Width(80)))
            {
                script.Rumble();
            }
        }

    }
}
