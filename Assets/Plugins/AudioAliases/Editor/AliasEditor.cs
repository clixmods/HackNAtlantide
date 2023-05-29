using System.Collections.Generic;
using System.Linq;
using AudioAliase;
using UnityEngine;
using UnityEditor;
using XMaterial;

namespace Audio.Editor
{
    [CustomEditor(typeof(Alias) , true)]
    public class AliasEditorDrawer : UnityEditor.Editor
    {
        int _numberTags;    
        private static string[] _tags;
             
        bool show3DSettings;
    
        private bool showSurfaceSettings;
        protected const string AliasesPropertyName = "aliases";
        protected const string AliasesTagCategoryName = "Audio Aliase/";
        public SerializedObject currentElemFromArraySelected;
        #region Serialized Properties
        public SerializedProperty nameAlias { get; private set; }
        public SerializedProperty description { get; private set; }
        public SerializedProperty mixerGroup { get; private set; }
        public SerializedProperty soundType { get; private set; }
        public SerializedProperty audio { get; private set; }
        public SerializedProperty secondaryAlias { get; private set; }
        public SerializedProperty bypassEffects { get; private set; }
        public SerializedProperty bypassListenerEffects { get; private set; }
        public SerializedProperty bypassReverbZones { get; private set; }
        public SerializedProperty priority { get; private set; }
        public SerializedProperty limitCount { get; private set; }
        public SerializedProperty minVolume { get; private set; }
        public SerializedProperty maxVolume { get; private set; }
        public SerializedProperty isLooping { get; private set; }
        public SerializedProperty minDelayLoop { get; private set; }
        public SerializedProperty maxDelayLoop { get; private set; }
        public SerializedProperty startAliase { get; private set; }
        public SerializedProperty endAliase { get; private set; }
        public SerializedProperty minPitch { get; private set; }
        public SerializedProperty maxPitch { get; private set; }
        public SerializedProperty stereoPan { get; private set; }
        public SerializedProperty spatialBlend { get; private set; }
        public SerializedProperty reverbZoneMix { get; private set; }
        public SerializedProperty textSubtitle { get; private set; }
        public SerializedProperty durationSubtitle { get; private set; }
        public SerializedProperty isInit { get; private set; }
        public SerializedProperty isPlaceholder { get; private set; }
        public SerializedProperty useSurfaceDetection { get; private set; }
        public SerializedProperty surfacesAlias { get; private set; }
        #endregion
    
    
        protected void DrawField(string propName, bool relative)
        {
            if (relative && currentElemFromArraySelected != null)
            {
                SerializedProperty sP = currentElemFromArraySelected.FindProperty(propName);
                EditorGUILayout.PropertyField(sP, true);
            }
            else if (currentElemFromArraySelected != null)
            {
                EditorGUILayout.PropertyField(currentElemFromArraySelected.FindProperty(propName),
                    true);
            }
        }
        
        public override void OnInspectorGUI()
        {
            _tags = GetTags();
            currentElemFromArraySelected = serializedObject;
            // switch (currentElemFromArraySelected.FindProperty("soundType").enumValueIndex)
            // {
            //         case (int) SoundType.Root:
            //             currentElemFromArraySelected.FindProperty("m_Script").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.FindAssets("AliasBase.cs")[0]);
            //             break;
            //         case (int) SoundType.Start:
            //             currentElemFromArraySelected.FindProperty("m_Script").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.FindAssets("AliasStart.cs")[0]);
            //             break;
            //         case (int) SoundType.End:
            //             currentElemFromArraySelected.FindProperty("m_Script").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.FindAssets("AliasEnd.cs")[0]);
            //             break;
            //         case (int)SoundType.Surface:
            //             currentElemFromArraySelected.FindProperty("m_Script").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.FindAssets("AliasSurface.cs")[0]);
            //             break;
            // }
            
        
            int guid = currentElemFromArraySelected.FindProperty("guid").intValue;
            DrawField("aliasName", true);
            DrawField("description", true);
            int selectedTag = GetIndexFromNameTag();
            selectedTag = EditorGUILayout.Popup("Tag", selectedTag, _tags);
            if (selectedTag == _tags.Length - 1 ) // Is Add Tag field
            {
                 EditorWindow.GetWindow<AddTagWindow>();
            }
            else
            {
                var tagProp = currentElemFromArraySelected.FindProperty("Tag");
                if (selectedTag == 0)
                    tagProp.stringValue = string.Empty;
                else
                    tagProp.stringValue = _tags[selectedTag];
            }
            DrawField("MixerGroup", true);
            DrawField("soundType",true);
            DrawField("audio", true);
            DrawField("randomizeClips", true);
            
            // var secondaryProp = currentElemFromArraySelected.FindProperty("Secondary");
            // int selectedAll = AliasUtilityEditor.GetIndexFrom(secondaryProp.intValue, AliasesEditorWindow.AliasesOptions);
            // selectedAll = EditorGUILayout.Popup("Secondary", selectedAll, AliasesEditorWindow.AliasesOptions.GetListDisplayName(guid).ToArray());
            //
            // DrawToolTip(secondaryProp);
            DrawField("Secondary", true);

            // if (selectedAll == 0 || AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindProperty("guid").intValue, AliasesEditorWindow.AliasesOptions ) == selectedAll)
            //     secondaryProp.intValue = 0;
            // else
            //     secondaryProp.intValue = AliasesEditorWindow.AliasesOptions[selectedAll];
            
            DrawField("bypassEffects", true);
            DrawField("bypassListenerEffects", true);
            DrawField("bypassReverbZones", true);
            DrawField("priority", true);
            
            DrawField("limitCount", true);
    
            // draw volume field
            SerializedProperty minVolume = currentElemFromArraySelected.FindProperty("minVolume");
            SerializedProperty maxVolume = currentElemFromArraySelected.FindProperty("maxVolume");
            DrawMinMaxSlider(minVolume, maxVolume, 0,1,"Volume");
            
            //float widthLabel = 75;
            // Only root sound can be looped sound
            if (currentElemFromArraySelected.FindProperty("soundType").enumValueIndex == (int) SoundType.Root)
            {
                // Loop part
                SerializedProperty sP = currentElemFromArraySelected.FindProperty("isLooping");
                sP.boolValue = EditorGUILayout.BeginToggleGroup("Is Looping", sP.boolValue);
                SerializedProperty minDelayLoop = currentElemFromArraySelected.FindProperty("minDelayLoop");
                SerializedProperty maxDelayLoop  = currentElemFromArraySelected.FindProperty("maxDelayLoop");
                DrawMinMaxSlider(minDelayLoop, maxDelayLoop, 0,60,"Delay Loop");
                // // Start alias
                // var startProp = currentElemFromArraySelected.FindProperty("startAliase");
                // int selectedStartAll = AliasUtilityEditor.GetIndexFrom(startProp.intValue ,AliasesEditorWindow.StartLoopAliasesOptions);
                // var displayNameStartAlias = AliasesEditorWindow.StartLoopAliasesOptions.GetListDisplayName(guid).ToArray();
                // selectedStartAll = EditorGUILayout.Popup("Start Aliase", selectedStartAll, displayNameStartAlias);
                // var guidValue = currentElemFromArraySelected.FindProperty("guid").intValue;
                // if (selectedStartAll == 0 || AliasUtilityEditor.GetIndexFrom(guidValue, AliasesEditorWindow.StartLoopAliasesOptions ) == selectedStartAll)
                //     startProp.intValue = 0;
                // else
                //     startProp.intValue = AliasesEditorWindow.StartLoopAliasesOptions[selectedStartAll];
                //
                DrawField("startAliase", true);
                DrawField("endAliase", true);
                // var endProp = currentElemFromArraySelected.FindProperty("endAliase");
                // int selectedendAll = AliasUtilityEditor.GetIndexFrom(endProp.intValue, AliasesEditorWindow.EndLoopAliasesOptions);
                // selectedendAll = EditorGUILayout.Popup("End Aliase", selectedendAll, AliasesEditorWindow.EndLoopAliasesOptions.GetListDisplayName(guid).ToArray());
                // if (selectedendAll == 0 || AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindProperty("guid").intValue, AliasesEditorWindow.EndLoopAliasesOptions ) == selectedendAll)
                //     endProp.intValue = 0;
                // else
                //     endProp.intValue = AliasesEditorWindow.EndLoopAliasesOptions[selectedendAll];
            
                EditorGUILayout.EndToggleGroup();
            }
    
            
            SerializedProperty minPitch = currentElemFromArraySelected.FindProperty("minPitch");
            SerializedProperty maxPitch = currentElemFromArraySelected.FindProperty("maxPitch");
            DrawMinMaxSlider(minPitch, maxPitch, -3,3,"Pitch");
            
            DrawField("stereoPan", true);
            DrawField("spatialBlend", true);
            DrawField("reverbZoneMix", true);
    
            #region 3D Setting
    
            ///// Draw the elem part with the arrow
            EditorGUILayout.BeginVertical("box");
            show3DSettings = EditorGUILayout.Foldout(show3DSettings, "3D Sound Settings");
            if (show3DSettings)
            {
                DrawField("dopplerLevel", true);
                DrawField("Spread", true);
                DrawField("MinDistance", true);
                DrawField("MaxDistance", true);
    
                SerializedProperty minDistance = currentElemFromArraySelected.FindProperty("MinDistance");
                SerializedProperty maxDistance = currentElemFromArraySelected.FindProperty("MaxDistance");
                float minDistanceValue = minDistance.floatValue;
                float maxDistanceValue = maxDistance.floatValue;
                EditorGUILayout.PrefixLabel("Min and Max distance");
                EditorGUILayout.MinMaxSlider(ref minDistanceValue, ref maxDistanceValue, 0, 10000);
                minDistance.floatValue = minDistanceValue;
                maxDistance.floatValue = maxDistanceValue;
    
                DrawField("CurveType", true);
                DrawField("distanceCurve", true);
            }
            EditorGUILayout.EndVertical();
    
            #endregion
            if (currentElemFromArraySelected.FindProperty("soundType").enumValueIndex == (int)SoundType.Root)
            {
                DrawField("Text", true);
                DrawField("customDuration", true);
            }
    
            DrawField("isInit", true);
            DrawField("isPlaceholder", true);
           
            
            if (currentElemFromArraySelected.FindProperty("soundType").enumValueIndex != (int)SoundType.Surface)
            {
                //#if XMATERIAL
                
                
                
                
                SerializedProperty sPUseSurface = currentElemFromArraySelected.FindProperty("UseSurfaceDetection");
                EditorGUILayout.LabelField("Surface Detection");
                DrawToolTip(sPUseSurface);
                sPUseSurface.boolValue = EditorGUILayout.BeginToggleGroup("UseSurfaceDetection", sPUseSurface.boolValue);
                if (sPUseSurface.boolValue)
                {
                    showSurfaceSettings = EditorGUILayout.Foldout(showSurfaceSettings, "Surface Settings");
                    if (showSurfaceSettings)
                    {
                    
                        for (int i = 0; i < XMaterialsData.SurfaceTypeNames.Length; i++)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(XMaterialsData.SurfaceTypeNames[i]);
                                var aliasSurface = currentElemFromArraySelected.FindProperty("surfacesAlias");
                                var structElem = aliasSurface.GetArrayElementAtIndex(i);
                                if (structElem == null)
                                {
                                    aliasSurface.InsertArrayElementAtIndex(i);
                                    structElem = aliasSurface.GetArrayElementAtIndex(i);
                                }
        
                                var stringSurface = structElem.FindPropertyRelative("surfaceName");
                                var guidAliasSurface = structElem.FindPropertyRelative("alias");
                                if (stringSurface.stringValue != XMaterialsData.SurfaceTypeNames[i])
                                {
                                    stringSurface.stringValue = XMaterialsData.SurfaceTypeNames[i];
                                }
                                EditorGUILayout.PropertyField(guidAliasSurface, true);
        
                                // int selectedSurface = AliasUtilityEditor.GetIndexFrom(guidAliasSurface.intValue, AliasesEditorWindow.SurfaceAliasesOptions);
                                // selectedSurface = EditorGUILayout.Popup("Alias", selectedSurface, AliasesEditorWindow.SurfaceAliasesOptions.GetListDisplayName(guid).ToArray());
                                // if (selectedSurface == 0 ||
                                //     AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindProperty("guid").intValue,
                                //         AliasesEditorWindow.SurfaceAliasesOptions) == selectedSurface)
                                //     guidAliasSurface.intValue = 0;
                                // else
                                //     guidAliasSurface.intValue = AliasesEditorWindow.SurfaceAliasesOptions[selectedSurface];
                            }
                        }
                       
                    }
                }
                EditorGUILayout.EndToggleGroup();
              
               // #endif
            }

            EditorGUILayout.TextField("GUID",currentElemFromArraySelected.FindProperty("guid").intValue.ToString());
            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawToolTip(SerializedProperty property)
        {
            // Draw Tooltip if its not drawed
            var typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", property.tooltip));
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
        private int GetIndexFromNameTag()
        {
            string lastString = _tags[_tags.Length - 1];
            for (int i = 0; i < _tags.Length; i++)
            {
                var tagProp = currentElemFromArraySelected.FindProperty("Tag");
                string valueFromProperty = tagProp.stringValue;
                if(valueFromProperty == lastString)
                {
                    tagProp.stringValue = string.Empty;
                }
    
                if (_tags[i] == tagProp.stringValue)
                    return i;
            }
    
    
            return 0;
        }
        
        protected void DrawMinMaxSlider(SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit , float maxLimit , string prefixLabel, string minLabel = "Min", string maxLabel = "Max", float widthLabel = 75)
        {
            float minValue = minProperty.floatValue;
            float maxValue = maxProperty.floatValue;
                
            EditorGUILayout.PrefixLabel(prefixLabel);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            
            minValue = EditorGUILayout.DelayedFloatField(minValue, GUILayout.Width(widthLabel));
            
            EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit);
            
            maxValue = EditorGUILayout.DelayedFloatField(maxValue, GUILayout.Width(widthLabel));
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField(minLabel);
            EditorGUILayout.LabelField(maxLabel);
            EditorGUILayout.EndHorizontal();
            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;
            
            DrawToolTip(minProperty);
        }
        
    }
}