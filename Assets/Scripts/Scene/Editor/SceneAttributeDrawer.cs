using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomPropertyDrawer(typeof(SceneAttribute))]

public class SceneAttributeDrawer : PropertyDrawer
{
    private string[] nameScenes;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string[] nameScenes = new string[EditorBuildSettings.scenes.Length] ;
        for(int i = 0; i < nameScenes.Length; i++)
        {
            SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[i].path);
            nameScenes[i] = asset.name;
        }
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                property.intValue =  EditorGUI.Popup(position,property.displayName,property.intValue, nameScenes);
                break;
            case SerializedPropertyType.String:
                int i = EditorGUI.Popup(position,GetIndexFromName(property.stringValue), nameScenes);
                 property.stringValue = nameScenes[i];
                break;
            default :
                EditorGUILayout.LabelField("Use Scene with Int or String");
                break;
        }
    } 
    int GetIndexFromName(string strToSearch)
    {
        int length = nameScenes.Length;
        for (int i = 0; i < length; i++)
        {
            if (nameScenes[i] == strToSearch)
                return i;
        }
        return 0;
    }
}
