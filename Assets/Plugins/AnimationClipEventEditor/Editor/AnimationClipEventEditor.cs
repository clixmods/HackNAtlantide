using System.Collections.Generic;
using Audio.Editor;
using AudioAliase;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

namespace Plugins.AnimationClipEventEditor.Editor
{
    public class AnimationClipEventEditor : EditorWindow
    {
        private static AnimationClip[] _animationClips;
        private static bool[] _showButtons;
        private static bool[] _showButtonsModels;
        private static Object[] _sourceModel;
        [MenuItem("Tools/Animation Event Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<AnimationClipEventEditor>();
            window.titleContent = new UnityEngine.GUIContent("Animation Event Editor");
            window.Show();
            GetAnimsWithEventAndGenerateButton();
        }

        private static void GetAnimsWithEventAndGenerateButton()
        {
            _animationClips = GetAllInstances();
            _showButtons = new bool[_animationClips.Length];

            int countAnimFromModel = 0;
            var guidsFBX = AssetDatabase.FindAssets("t:Model", new[] {"Assets"});
            ;
            foreach (var guid in guidsFBX)
            {
                var asset = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
                if (asset is ModelImporter importer)
                {
                    countAnimFromModel += importer.clipAnimations.Length;
                }
            }

            _showButtonsModels = new bool[countAnimFromModel];
        }

        private void OnGUI()
        {
            if (_animationClips == null)
            {
                GetAnimsWithEventAndGenerateButton();
            }


            for (int i = 0; i < _animationClips.Length; i++)
            {
                var animationCLip = _animationClips[i];
                using (new GUILayout.VerticalScope())
                {
                    _showButtons[i] = EditorGUILayout.Foldout(_showButtons[i], animationCLip.name);
                    if (_showButtons[i])
                    {
                        EditorGUI.indentLevel++;
                        var events = animationCLip.events;
                        using (new GUILayout.VerticalScope())
                        {
                            for (int j = 0; j < events.Length; j++)
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    AnimationEvent animationEvent = events[j];
                                    if (animationEvent != null)
                                    {
                                        EditorGUILayout.LabelField(
                                            $"Frame : {animationEvent.time * animationCLip.frameRate}");
                                        if (animationEvent.functionName.Contains("Alias"))
                                        {
                                            animationEvent.intParameter = guidAliases[DrawPopupAlias(animationEvent.intParameter)];
                                        }
                                    }
                                   
                                }
                            }
                        }

                        AnimationUtility.SetAnimationEvents(animationCLip, events);

                        EditorGUI.indentLevel--;
                    }
                }
            }
            int indexButton = -1;
            var guidsFBX = AssetDatabase.FindAssets("t:Model", new[] {"Assets"});
            _sourceModel = new Object[guidsFBX.Length];
            for(int index = 0 ; index < guidsFBX.Length; index++)
            {
                string guid = guidsFBX[index];
                var asset = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
                if (asset is ModelImporter importer)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(asset.name);
                    EditorGUI.indentLevel++;
                   
                    var clipAnimations = importer.clipAnimations;
                   
                    for (int i = 0; i < clipAnimations.Length; i++)
                    {
                        var clipAnimation = clipAnimations[i];
                        using (new GUILayout.VerticalScope())
                        {
                            indexButton++;
                            
                            _showButtonsModels[indexButton] = EditorGUILayout.Foldout(_showButtonsModels[indexButton], clipAnimation.name);
                            if (_showButtonsModels[indexButton])
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
                                                    animationEvent.intParameter =
                                                        guidAliases[DrawPopupAlias(animationEvent.intParameter)];
                                                }

                                                // End the code block and update the label if a change occurred
                                                if (EditorGUI.EndChangeCheck())
                                                {
                                                    importer.clipAnimations = clipAnimations;
                                                    importer.SaveAndReimport();
                                                }
                                            }
                                        }
                                    }
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        static AnimationClip[] GetAllInstances()
        {
            string[]
                guids = AssetDatabase.FindAssets("t:" + typeof(AnimationClip)
                    .Name); //FindAssets uses tags check documentation for more info
            int count = guids.Length;
            AnimationClip[] a = new AnimationClip[count];
            for (int i = 0; i < count; i++) //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            }

            return a;
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
}