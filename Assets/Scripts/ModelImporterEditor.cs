using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Audio.Editor;
using AudioAliase;
using UnityEditor.AssetImporters;
using UnityEditor.Experimental.AssetImporters;


//[CustomEditor(typeof(ModelImporter))]
public class ModelImporterEditor : AssetImporterEditor
{
    Editor defaultEditor;

    public override void OnEnable()
    {
        base.OnEnable();
        defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.ModelImporterEditor, UnityEditor"));

    }

    public override void OnDisable()
    {
        base.OnDisable();
//        DestroyImmediate(defaultEditor);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        defaultEditor.OnInspectorGUI();

        ApplyRevertGUI();
        GUILayout.Label("Alias animation events");
        //if (GUILayout.Button("CREATE LODs"))
        {
            ModelImporter myTarget = (ModelImporter) target;
            var clipAnimations = myTarget.clipAnimations;
            for (int i = 0; i < clipAnimations.Length; i++)
            {
                var clipAnimation = clipAnimations[i];
                using (new GUILayout.VerticalScope())
                {
                    EditorGUI.indentLevel++;
                    using (new GUILayout.VerticalScope())
                    {
                        for (int j = 0; j < clipAnimation.events.Length; j++)
                        {
                            AnimationEvent animationEvent = clipAnimation.events[j];
                            if (animationEvent != null)
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.LabelField(
                                        $"Frame : {animationEvent.time}");
                                    if (animationEvent.functionName.Contains("Alias"))
                                    {
                                        animationEvent.intParameter = guidAliases[DrawPopupAlias(animationEvent.intParameter)];
                                    }
                                }
                            }
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
    
    
    private List<string> nameScenes;
    private List<int> guidAliases;
    private AliasesScriptableObject[] _aliasesArray;

    public int DrawPopupAlias(int intValue)
    {
        using (new GUILayout.HorizontalScope(GUILayout.MinWidth(10), GUILayout.MaxWidth(350)))
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

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_editicon.sml")))
            {
                AliasesEditorWindow.Open();
            }

            int index = EditorGUILayout.Popup("Alias to Play",
                AliasUtilityEditor.GetIndexFrom(intValue, guidAliases),
                nameScenes.ToArray());
            return index;
        }
    }
    private static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[]
            guids = AssetDatabase.FindAssets("t:" +
                                             typeof(T)
                                                 .Name); //FindAssets uses tags check documentation for more info
        int count = guids.Length;
        T[] a = new T[count];
        for (int i = 0; i < count; i++) //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

}