using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace AudioAliase
{
    [CreateAssetMenu(fileName = "new_aliases", menuName = "Audio/aliases", order = 1)]
    public class AliasesScriptableObject : ScriptableObject
    {
        public bool DontLoad;
        public AudioMixerGroup defaultMixerGroup;
        public List<Alias> aliases;
        public Dictionary<string, Queue<Alias>> aliasesDictionnary;
        private void OnValidate()
        {
            if (DontLoad == true)
                return;
            
            //AudioManager.AddAliases(this);
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

                alias.dictSurfacesAlias = new Dictionary<string, int>();
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
