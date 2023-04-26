using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GameState),true)]
public class GameStatePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string name = property.type;
        int index = name.IndexOf('<');

        if(index != -1)
        {
            name = name.Remove(0,index+1);
        }

        index = name.IndexOf('>');

        if(index!=-1)
        {
            name = name.Remove(index);
        }

        EditorGUI.LabelField(position,ObjectNames.NicifyVariableName(name));
    }
}
