using System;
using System.Collections.Generic;
using System.Reflection;
using AudioAliase;
using XEditor.Editor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio.Editor
{
    
    public class AliasesEditorWindow : ExtendedEditorWindow
    {
        
        Vector2 scrollPos;
        private static AliasesScriptableObject[] _aliasesArray = new AliasesScriptableObject[0];
        private string _searchValue;
        bool _fold;
        Transform selectedTransform;
        private List<string> _options;
        /// <summary>
        /// List of all aliases created in the project
        /// </summary>
        public static List<int> _aliases;
        /// <summary>
        /// List of all aliases created in the project for selection with none value
        /// </summary>
        public static List<int> _aliasesOptions;
        /// <summary>
        /// List of all start aliases (used in loop) created in the project for selection with none value
        /// </summary>
        public static List<int> _startLoopAliasesOptions;
        /// <summary>
        /// List of all end aliases (used in loop) created in the project for selection with none value
        /// </summary>
        public static List<int> _endLoopAliasesOptions;
        /// <summary>
        /// List of all surface alias created in the project for selection with none value
        /// </summary>
        public static List<int> _surfaceAliasesOptions;


        public static Dictionary<int, Alias> Dictionary = new Dictionary<int, Alias>();


        // Constant string
        private const string TAG_MNGR_ASSET = "ProjectSettings/TagManager.asset";
        private const string ASSET_ALREADY_EXIST = "An asset exists already with the name, change it";
        int _numberTags;    
        private string[] _tags;
        int _selected = 0;
        string newAliaseName = "New aliases file name";
        //FILTER
        bool _showOnlyPlaceHolder;
        int _selectedTagIndex = 0;
        int _selectedSoundTypeIndex = 0;
        bool _showLooped;
        bool _showSurfaceDetection;
        bool show3DSettings;
        private string _message;
        private bool showSurfaceSettings;
        private AudioSource _audioSource;
        // Method executed when we open the window
        [MenuItem("Tools/Audio Aliases")]
        public static void Open()
        {
            AliasesEditorWindow window = GetWindow<AliasesEditorWindow>("Audio Aliases Editor");
            window.titleContent.image = EditorGUIUtility.IconContent("d_AudioImporter Icon").image;
            window.UpdateAliasesFileList();
            window.UpdateAliasesList();
            window.UpdateTagList();
            //GameObject oof = new GameObject();
            
           // window._audioSource = oof.AddComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            if(_audioSource != null)
                DestroyImmediate(_audioSource.gameObject);
        }

        public void UpdateTagList()
        {
            _tags = GetTags();
        }
        public void SetupAudioSource(Alias alias)
        {
            _audioSource.clip = alias.Audio;
            _audioSource.volume = Random.Range(alias.minVolume, alias.maxVolume);
            _audioSource.loop = alias.isLooping;
            _audioSource.pitch = Random.Range(alias.minPitch, alias.maxPitch);
            
            _audioSource.spatialBlend = alias.spatialBlend;
            if (alias.MixerGroup != null)
                _audioSource.outputAudioMixerGroup = alias.MixerGroup;

            switch (alias.CurveType)
            {
                case AudioRolloffMode.Logarithmic:
                case AudioRolloffMode.Linear:
                    _audioSource.rolloffMode = alias.CurveType;
                    break;
                case AudioRolloffMode.Custom:
                    _audioSource.rolloffMode = alias.CurveType;
                    _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, alias.distanceCurve);
                    break;

            }
        }
        void UpdateAliasesFileList()
        {
            serializedObject = new List<SerializedObject>();
            // We get all aliases available in the project, to convert them into serializedObject
            _aliasesArray = XEditorUtility.GetAssets<AliasesScriptableObject>();
            foreach (AliasesScriptableObject asset in _aliasesArray)
            {
                var newSerializedObject = new SerializedObject(asset);
                serializedObject.Add(newSerializedObject);
            }
            selectedSerializeObject = null;
            Repaint();
        }
        // This method is executed each frame
        private void OnGUI()
        {
            // Verification before draw the window
            if (serializedObject == null)
            {
                Debug.Log("Warning : Aliase Editor have no serializedObject");
                UpdateAliasesFileList();
                UpdateAliasesList();
                UpdateTagList();
                return;
            }
            // We draw the visual of the window
            // First Begin horizontal to draw : left to right
            using (new GUILayout.HorizontalScope())
            {
                DrawLeftPanel();
                DrawMiddlePanel();
                DrawRightPanel();
            }
            Apply();
        }

        /// <summary>
        /// This method will update the list of aliases, need to be called when its required
        /// </summary>
        private void UpdateAliasesList()
        {
            _aliases = new List<int>();
            //
            _aliasesOptions = new List<int>();
            _aliasesOptions.Add(0);
            //
            _startLoopAliasesOptions = new List<int>();
            _startLoopAliasesOptions.Add(0);
            //
            _endLoopAliasesOptions = new List<int>();
            _endLoopAliasesOptions.Add(0);
            //
            _surfaceAliasesOptions = new List<int>();
            _surfaceAliasesOptions.Add(0);
            //

            var listoof = GetAllInstances<AliasesScriptableObject>();
            foreach (var aliases in listoof)
            {
                foreach (var alias in aliases.aliases)
                {
                    Dictionary[alias.GUID] = alias;
                }
            }
           
             static T[] GetAllInstances<T>() where T : ScriptableObject
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
            
            foreach (SerializedObject o in serializedObject)
            {
                var aliasesListProp = o.FindProperty(AliasesPropertyName);
                
                foreach (SerializedProperty p in aliasesListProp)
                {
                    var aliasGUID = p.FindPropertyRelative("guid").intValue;
                    _aliases.Add(aliasGUID);
                    if (p.FindPropertyRelative("soundType").enumValueIndex == (int) SoundType.Root)
                    {
                        _aliasesOptions.Add(aliasGUID);
                    }
                    if (p.FindPropertyRelative("soundType").enumValueIndex == (int) SoundType.Start)
                    {
                        _startLoopAliasesOptions.Add(aliasGUID);
                    }
                    else if (p.FindPropertyRelative("soundType").enumValueIndex == (int) SoundType.End)
                    {
                        _endLoopAliasesOptions.Add(aliasGUID);
                    }
                    else if (p.FindPropertyRelative("soundType").enumValueIndex == (int) SoundType.Surface)
                    {
                        _surfaceAliasesOptions.Add(aliasGUID);
                    }
                }
            }
            Apply();
            Repaint();
        }
        void DrawMiddlePanel()
        {
            // Middle part
            EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(150), GUILayout.MaxWidth(750), GUILayout.ExpandHeight(true));
            // Allow the possibility to use a scrollbar to navigate
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
            EditorGUILayout.LabelField("Search");
            _searchValue = EditorGUILayout.TextField(_searchValue);
            // if a Aliase file is selected, go draw all properties
            if (selectedSerializeObject != null)
            {
                // Among serializeObjects, we search aliases list for the selected 
                if(showAll)
                {
                    for (int i = 0; i < serializedObject.Count; i++)
                    {
                        currentProperty = serializedObject[i].FindProperty(AliasesPropertyName);
                        DrawTheShit();
                    }
                }
                else
                {
                    currentProperty = selectedSerializeObject.FindProperty(AliasesPropertyName);
                    DrawTheShit();
                    if (GUILayout.Button("Create aliase"))
                    {
                        currentProperty.InsertArrayElementAtIndex(currentProperty.arraySize);
                        ApplyAliaseDefaultValue(currentProperty.GetArrayElementAtIndex(currentProperty.arraySize-1));
                        
                        return;
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Please select a aliases list", MessageType.Info);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void ApplyAliaseDefaultValue(SerializedProperty serializedProperty)
        {
            serializedProperty.FindPropertyRelative("name").stringValue = "NewAliase"; 
            serializedProperty.FindPropertyRelative("minPitch").floatValue = 1;
            serializedProperty.FindPropertyRelative("maxPitch").floatValue = 1.01f;
            serializedProperty.FindPropertyRelative("MinDistance").floatValue = 0;
            serializedProperty.FindPropertyRelative("MaxDistance").floatValue = 500;
            serializedProperty.FindPropertyRelative("minVolume").floatValue = 0.8f;
            serializedProperty.FindPropertyRelative("maxVolume").floatValue = 0.8f;
            serializedProperty.FindPropertyRelative("reverbZoneMix").floatValue = 1;
            serializedProperty.FindPropertyRelative("dopplerLevel").floatValue = 1;
            serializedProperty.FindPropertyRelative("Spread").floatValue = 1;
            serializedProperty.FindPropertyRelative("isPlaceholder").boolValue = true;
            serializedProperty.FindPropertyRelative("guid").intValue = AliasUtility.GenerateID();

            //serializedProperty.FindPropertyRelative("delayLoop").floatValue = 0;

        }
        void DrawTheShit()
        {
            // We draw the content of aliases    
            // Navigate between each elem of the list aliases
            //foreach (SerializedProperty p in currentProperty)
            for (int i = 0; i < currentProperty.arraySize; i++)
            {
               SerializedProperty p = currentProperty.GetArrayElementAtIndex(i);
               currentElemFromArraySelected = p;
               if (currentElemFromArraySelected == null)
                   continue;
               
                if ( !String.IsNullOrEmpty(_searchValue) && !currentElemFromArraySelected.FindPropertyRelative("name").stringValue.Contains(_searchValue))
                {
                    continue;
                }
                // check filter
                if (_showOnlyPlaceHolder && !currentElemFromArraySelected.FindPropertyRelative("isPlaceholder").boolValue)
                {
                    continue;
                }
                if ( _selectedTagIndex != 0 && _tags[_selectedTagIndex] != currentElemFromArraySelected.FindPropertyRelative("Tag").stringValue)
                {
                    continue;
                }
                //if(_selectedSoundTypeIndex != 0 && _tags[_selectedTagIndex] != currentElemFromArraySelected.FindPropertyRelative("SoundType").stringValue )
                EditorGUILayout.BeginVertical("HelpBox");
                GUIContent spatialRightLabel = EditorGUIUtility.TrTextContent("-","Remove the shi");
                // If I understand correctly, FindPropertyRelative try to find the property declared into arg,
                // Maybe a way to check property inside the elem of the array etc 
                // _options.Add(currentElemFromArraySelected.FindPropertyRelative("name").stringValue);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(spatialRightLabel, GUILayout.Width(30)))
                {
                    var title = "Delete the selection";
                    _message = "Are you sure you want to delete?";
                    var ok = "Delete";
                    var cancel = "No";
                    if( EditorUtility.DisplayDialog(title, _message, ok, cancel))
                    {
                        currentProperty.DeleteArrayElementAtIndex(i);
                    }
                }
                // Draw the elem part with the arrow
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();
                if (  p.isExpanded )
                {
                    EditorGUI.indentLevel++;
                    //DrawSelectedPanel();
                    EditorGUILayout.PropertyField(currentElemFromArraySelected, true);
                    EditorGUI.indentLevel--;
                }
        
                EditorGUILayout.EndVertical();

            }
        }
      

        void DrawLeftPanel()
        {
            // Left side of the window:
            // Shows every aliases asset with options
            using (new GUILayout.VerticalScope(EditorStyles.whiteLabel))
            {
                // Filter part
                // a text, usefull if we want give description to some properties
                EditorGUILayout.LabelField("Filter");
                // A checkbox for the bool showOnlyPlaceholder
                _showOnlyPlaceHolder = EditorGUILayout.Toggle("Show only placeholder", _showOnlyPlaceHolder);
                _selectedTagIndex = EditorGUILayout.Popup("Tag", _selectedTagIndex, _tags);
                _selectedSoundTypeIndex = EditorGUILayout.Popup("Sound type", _selectedSoundTypeIndex, _tags);
                _showLooped = EditorGUILayout.Toggle("Show loop only", _showLooped);
                _showSurfaceDetection = EditorGUILayout.Toggle("Show surface detection only", _showSurfaceDetection);
                // Aliases part
                if (EditorGUILayout.BeginFadeGroup(1))
                {
                    EditorGUILayout.LabelField("Aliases Files");
                }
                // We draw a sidebar for each file converted into SerializedObject
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button($" All Count : {_aliases.Count}"))
                {
                    showAll = true;
                }
                EditorGUILayout.EndHorizontal();
                foreach (SerializedObject prop in serializedObject)
                {
                    DrawSidebar(prop);
                }
                EditorGUILayout.EndFadeGroup();
                // Begin to draw a line of button after the list of aliases file
                EditorGUILayout.Space(10);
                if (GUILayout.Button("Create new aliases file"))
                {
                    CreateNewAliasesFile();
                }
            } 
        }
        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical();
            _selected = EditorGUILayout.Popup("Sound", _selected, _aliasesOptions.GetListDisplayName().ToArray());
            if (GUILayout.Button("Play sound"))
            {
                int aliaseGUID = _aliasesOptions[_selected];
                AudioManager.PlaySoundAtPosition(aliaseGUID, Camera.main.transform.position);
                this.ShowNotification(new GUIContent($"Sound {aliaseGUID} played"));
            }

            EditorGUILayout.FloatField(0.5f);
            if (GUILayout.Button("Play in loop"))
            {
            }
            EditorGUILayout.EndVertical();
        }
        private string[] GetTags()
        {
            _numberTags = UnityEditorInternal.InternalEditorUtility.tags.Length + 2;
            
            string[] temptagarray = new string[_numberTags];
            int newSize = 0;
            temptagarray[newSize] = "None";
            newSize++;
            for (int i = 1; i < _numberTags - 1; i++)
            {
                string tagSelected = UnityEditorInternal.InternalEditorUtility.tags[i - 1];
                if (tagSelected.Contains(AliasesTagCategoryName))
                {
                    temptagarray[newSize] = tagSelected;
                    newSize++;
                }
            }
            newSize++;
            string[] tagsAll = new string[newSize];
            temptagarray[newSize - 1] = "Add tag..";
            for (int i = 0; i < newSize; i++)
            {
                tagsAll[i] = temptagarray[i];
            }
            return tagsAll;
        }
        protected override void Apply()
        {
            foreach (SerializedObject asset in serializedObject)
            {   
                asset.ApplyModifiedProperties();
            }
        }
        string IsValidName(string nameToValidate)
        {
            return nameToValidate;
            
        }
        /// <summary>
        /// Create a new file to contain aliase list,
        /// need to be created in the folder where the previous file is created
        /// </summary>
        void CreateNewAliasesFile()
        {
            AliasesScriptableObject instance = ScriptableObject.CreateInstance<AliasesScriptableObject>();
            string path = $"Assets/{IsValidName(newAliaseName)}.asset";
            
            var oof  = EditorUtility.SaveFilePanelInProject(
                "Save aliase file",
                $"oof",
                "asset",
                "asset");
            
            if (!string.IsNullOrEmpty(oof))
            {
                UnityEditor.AssetDatabase.CreateAsset(CreateInstance<AliasesScriptableObject>(), oof);
                AssetDatabase.Refresh();
            }
            
            UpdateAliasesFileList(); // Update the left panel
        }
    }
    
}
