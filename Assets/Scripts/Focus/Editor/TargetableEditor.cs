using UnityEditor;
using UnityEngine;


// //[CustomEditor(typeof(Targetable))]
//     public class TargetableEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("targeter"),true);
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("useDistance"),true);
//             GUI.enabled = serializedObject.FindProperty("useDistance").boolValue;
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("maxDistanceWithTargeter"),true);
//             GUI.enabled = true;
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("canBeTarget"),true);
//             serializedObject.ApplyModifiedProperties();
//         }
//     }
