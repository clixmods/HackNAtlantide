using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace AudioAliase
{
    [CreateAssetMenu(fileName = "new_aliases", menuName = "Audio/aliases", order = 1)]
    public class AliasesScriptableObject : ScriptableObject
    {
        public bool DontLoad;
        public AudioMixerGroup defaultMixerGroup;
        public List<AliasClassBullshit> aliases;
        public List<Alias> AliasesSubAsset;
        public Dictionary<string, Queue<Alias>> aliasesDictionnary;
        public bool ConvertAliasToScriptableObject;
        private void OnValidate()
        {
            this.AliasesSubAsset = GetSubObjectsOfType<Alias>(this);
            
            if (DontLoad == true)
                return;
            if (ConvertAliasToScriptableObject)
            {
                var assetPath = AssetDatabase.GetAssetPath(this);
                //var AssetDatabase.LoadAssetAtPath<AliasesScriptableObject>(assetPath);
                // Add an animation clip to it
                
                // Create a simple material asset

//                AliasesScriptableObject aliasesScriptableObject = ScriptableObject.CreateInstance<AliasesScriptableObject>();
  //              aliasesScriptableObject.name = name;
                // AssetDatabase.CreateAsset(aliasesScriptableObject, $"Assets/{name}.asset");
                //
                //
                //
                //
                // foreach (var aliasClass in aliases)
                // {
                //     Alias instance = ScriptableObject.CreateInstance<Alias>();
                //     ConvertAliasClassToScriptable(instance, aliasClass);
                //     string path = $"Assets/{aliasClass.name}.asset";
                //     
                //     AssetDatabase.AddObjectToAsset(instance, aliasesScriptableObject);
                //
                //     // Reimport the asset after adding an object.
                //     // Otherwise the change only shows up when saving the project
                //     AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(instance));
                //     AssetDatabase.Refresh();
                //     
                // }
                AliasesSubAsset = GetSubObjectsOfType<Alias>(this);
                foreach (Alias aliasSubAsset in AliasesSubAsset)
                {
                   // Alias alias = ScriptableObject.CreateInstance<Alias>();
                   AssetDatabase.RemoveObjectFromAsset(aliasSubAsset);
                    AssetDatabase.CreateAsset(aliasSubAsset, $"Assets/{aliasSubAsset.name}.asset");
                }
                ConvertAliasToScriptableObject = false;
            }
            //AudioManager.AddAliases(this);
        }
        public static List<T> GetSubObjectsOfType<T>(Object asset) where T : Object{
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(asset));
            List<T> ofType = new List<T>();
            foreach(Object o in objs){
                if(o is T o1){
                    ofType.Add(o1);
                }
            }
            return ofType;
        }

        private static void ConvertAliasClassToScriptable(Alias instance, AliasClassBullshit aliasClass)
        {
            instance.name = aliasClass.name;
            instance.aliasName = aliasClass.name;
            instance.description = aliasClass.description;
            instance.Tag = aliasClass.Tag;

            instance.MixerGroup = aliasClass.MixerGroup;
            instance.audio = aliasClass.audio;

            instance.soundType = aliasClass.soundType;


            instance.randomizeClips = aliasClass.randomizeClips;


            //instance.Secondary = aliasClass.Secondary;

            instance.bypassEffects = aliasClass.bypassEffects;
            instance.bypassListenerEffects = aliasClass.bypassListenerEffects;
            instance.bypassReverbZones = aliasClass.bypassReverbZones;
            instance.priority = aliasClass.priority;
            //instance.volume = aliasClass.volume;
            instance.minVolume = aliasClass.minVolume;
            instance.maxVolume = aliasClass.maxVolume;
            instance.limitCount = aliasClass.limitCount;

            instance.isLooping = aliasClass.isLooping;

            //instance.startAliase = aliasClass.startAliase;

           // instance.endAliase = aliasClass.endAliase;
            instance.UseDelayLoop = aliasClass.UseDelayLoop;
            instance.minDelayLoop = aliasClass.minDelayLoop;
            instance.maxDelayLoop = aliasClass.maxDelayLoop;

            instance.minPitch = aliasClass.minPitch;
            instance.maxPitch = aliasClass.maxPitch;
            instance.stereoPan = aliasClass.stereoPan;
            instance.spatialBlend = aliasClass.spatialBlend;

            instance.reverbZoneMix = aliasClass.reverbZoneMix;
            instance.dopplerLevel = aliasClass.dopplerLevel;
            instance.Spread = aliasClass.Spread;
            instance.MinDistance = aliasClass.MinDistance;
            instance.MaxDistance = aliasClass.MaxDistance;
            instance.CurveType = aliasClass.CurveType;
            instance.distanceCurve = aliasClass.distanceCurve;
            instance.Text = aliasClass.Text;
            instance.customDuration = aliasClass.customDuration;

            instance.isInit = aliasClass.isInit;
            instance.isPlaceholder = aliasClass.isPlaceholder;
            instance.audioPlayers = aliasClass.audioPlayers;
            instance.UseSurfaceDetection = aliasClass.UseSurfaceDetection;
            instance.surfacesAlias = aliasClass.surfacesAlias;
           // instance.dictSurfacesAlias = aliasClass.dictSurfacesAlias;
            instance.guid = aliasClass.GUID;
        }

        private void OnDisable()
        {
            foreach (var VARIABLE in aliases)
            {
                VARIABLE.audioPlayers.Clear();
            }
        }

        private void OnEnable()
        {
            foreach (var alias in aliases)
            {
                alias.audioPlayers.Clear();

                alias.dictSurfacesAlias = new Dictionary<string, Alias>();
                if (alias.surfacesAlias != null)
                {
                    for (int i = 0; i < alias.surfacesAlias.Length ; i++)
                    {
                        alias.dictSurfacesAlias[alias.surfacesAlias[i].surfaceName] = alias.surfacesAlias[i].alias;
                    }
                }
                
            }
        }

        private void Awake()
        {
            //AudioManager.AddAliases(this);
        }
        
    }
    
}
