using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace AudioAliase
{
    [CreateAssetMenu(fileName = "AudioManager Data", menuName = "Audio/Audio Manager Data", order = 2)]
    public class AudioManagerData : ScriptableObject
    {
        public AliasesScriptableObject[] aliases;
        public bool debugMessage;
        public Dictionary<int, Alias> DictionaryAliases = new Dictionary<int, Alias>();
        #if UNITY_EDITOR
            private void OnValidate()
            {
                aliases = GetAllInstances();
            }
            private static AliasesScriptableObject[] GetAllInstances()
            {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(AliasesScriptableObject).Name);  //FindAssets uses tags check documentation for more info
                int count = guids.Length;
                AliasesScriptableObject[] a = new AliasesScriptableObject[count];
                for (int i = 0; i < count; i++)         //probably could get optimized 
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var asset = (AliasesScriptableObject) AssetDatabase.LoadAssetAtPath<AliasesScriptableObject>(path);
                    if(!asset.DontLoad)
                        a[i] = AssetDatabase.LoadAssetAtPath<AliasesScriptableObject>(path);
                }

                return a;

            }
        #endif

        private void OnEnable()
        {
            for (int i = 0; i < aliases.Length; i++)
            {
                AliasesScriptableObject currentAliasesScriptableObject = aliases[i];
                for (int j = 0; j < aliases[i].aliases.Count; j++)
                {
                    Alias currentAlias = currentAliasesScriptableObject.aliases[j];
                    DictionaryAliases[currentAlias.GUID] = currentAlias;
                }
            }
        }
    }
}