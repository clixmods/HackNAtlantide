using System.Collections.Generic;
using AudioAliase;
using UnityEditor;
using UnityEngine;

namespace Audio.Editor
{
    public static class StringAliasExtension
    {
        public static void NullSound(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                str = AudioManager.AliasNameNull;
            }
        }
    }
    
    //TODO : NEED TO OPTIMIZE CAUSE BAD FRAMERATE
    [CustomPropertyDrawer(typeof(AliaseAttribute))]
    public class AliaseAttributeDrawer : PropertyDrawer
    {
        private List<string> nameScenes;
        private List<int> guidAliases;
        private AliasesScriptableObject[] _aliasesArray;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_aliasesArray == null || _aliasesArray.Length == 0)
            {
                _aliasesArray = GetAllInstances<AliasesScriptableObject>();
                Debug.Log("Aliase get in attribute");
            }
            nameScenes = new List<string>();
            guidAliases = new List<int>();
            // Index 0 is null name
            nameScenes.Add(AudioManager.AliasNameNull);
            guidAliases.Add(0);
            foreach (AliasesScriptableObject asset in _aliasesArray)
            {
                foreach (var alias in asset.aliases)
                {
                    nameScenes.Add(alias.name);
                    guidAliases.Add(alias.GUID);
                }
            }
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    //property.intValue = EditorGUILayout.Popup(property.intValue, nameScenes.ToArray());
                    var buttonPosition = position;
                    buttonPosition.x = position.width-1;
                    buttonPosition.width = 25;
                   // buttonPosition.height = 25;
                    if (GUI.Button(buttonPosition,EditorGUIUtility.IconContent("d_editicon.sml")))
                    {
                        AliasesEditorWindow.Open();
                    }

                    position.width -= buttonPosition.width;
                    int index = EditorGUI.Popup(position, property.displayName ,AliasUtilityEditor.GetIndexFrom(property.intValue, guidAliases), nameScenes.ToArray());
                    property.intValue = guidAliases[index];
                    break;
                default :
                    EditorGUILayout.LabelField("Use Scene with Int"); 
                    break;
            }
            
            
        }
        
        
        private static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            int count = guids.Length;
            T[] a = new T[count];
            for (int i = 0; i < count; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;

        }
    }
}