using System.Collections.Generic;
using System.Linq;
using AudioAliase;
using UnityEngine;
using UnityEditor;
using XMaterial;

namespace Audio.Editor
{
    [CustomPropertyDrawer(typeof(Alias))]
    public class AliasEditorDrawer : PropertyDrawer
    {
        int _numberTags;    
        private static string[] _tags;
             
        bool show3DSettings;
    
        private bool showSurfaceSettings;
        protected const string AliasesPropertyName = "aliases";
        protected const string AliasesTagCategoryName = "Audio Aliase/";
        public SerializedProperty currentElemFromArraySelected;
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
                SerializedProperty sP = currentElemFromArraySelected.FindPropertyRelative(propName);
                EditorGUILayout.PropertyField(sP, true);
            }
            else if (currentElemFromArraySelected.serializedObject != null)
            {
                EditorGUILayout.PropertyField(currentElemFromArraySelected.serializedObject.FindProperty(propName),
                    true);
            }
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //var window = UnityEditor.EditorWindow.GetWindow<AliasesEditorWindow>();
            if (!EditorWindow.HasOpenInstances<AliasesEditorWindow>())
            {
                base.OnGUI(position, property, label);
                if (GUI.Button(position,"Open Alias Editor"))
                {
                    AliasesEditorWindow.Open();
                }
                return;
            }
            _tags = GetTags();
            currentElemFromArraySelected = property;
            DrawField("name", true);
            DrawField("description", true);
            int selectedTag = GetIndexFromNameTag();
            selectedTag = EditorGUILayout.Popup("Tag", selectedTag, _tags);
            if (selectedTag == _tags.Length - 1 ) // Is Add Tag field
            {
                 EditorWindow.GetWindow<AddTagWindow>();
            }
            else
            {
                var tagProp = currentElemFromArraySelected.FindPropertyRelative("Tag");
                if (selectedTag == 0)
                    tagProp.stringValue = string.Empty;
                else
                    tagProp.stringValue = _tags[selectedTag];
            }
            DrawField("MixerGroup", true);
            DrawField("soundType",true);
            DrawField("audio", true);
            DrawField("randomizeClips", true);
            
            var secondaryProp = currentElemFromArraySelected.FindPropertyRelative("Secondary");
            int selectedAll = AliasUtilityEditor.GetIndexFrom(secondaryProp.intValue, AliasesEditorWindow._aliasesOptions);
            selectedAll = EditorGUILayout.Popup("Secondary", selectedAll, AliasesEditorWindow._aliasesOptions.GetListDisplayName().ToArray());
           
            DrawToolTip(secondaryProp);

            if (selectedAll == 0 || AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindPropertyRelative("guid").intValue, AliasesEditorWindow._aliasesOptions ) == selectedAll)
                secondaryProp.intValue = 0;
            else
                secondaryProp.intValue = AliasesEditorWindow._aliasesOptions[selectedAll];
            
            DrawField("bypassEffects", true);
            DrawField("bypassListenerEffects", true);
            DrawField("bypassReverbZones", true);
            DrawField("priority", true);
            
            DrawField("limitCount", true);
    
            // draw volume field
            SerializedProperty minVolume = currentElemFromArraySelected.FindPropertyRelative("minVolume");
            SerializedProperty maxVolume = currentElemFromArraySelected.FindPropertyRelative("maxVolume");
            DrawMinMaxSlider(minVolume, maxVolume, 0,1,"Volume");
            
            //float widthLabel = 75;
            // Only root sound can be looped sound
            if (currentElemFromArraySelected.FindPropertyRelative("soundType").enumValueIndex == (int) SoundType.Root)
            {
                // Loop part
                SerializedProperty sP = currentElemFromArraySelected.FindPropertyRelative("isLooping");
                sP.boolValue = EditorGUILayout.BeginToggleGroup("Is Looping", sP.boolValue);
                SerializedProperty minDelayLoop = currentElemFromArraySelected.FindPropertyRelative("minDelayLoop");
                SerializedProperty maxDelayLoop  = currentElemFromArraySelected.FindPropertyRelative("maxDelayLoop");
                DrawMinMaxSlider(minDelayLoop, maxDelayLoop, 0,60,"Delay Loop");
                // Start alias
                var startProp = currentElemFromArraySelected.FindPropertyRelative("startAliase");
                int selectedStartAll = AliasUtilityEditor.GetIndexFrom(startProp.intValue ,AliasesEditorWindow._startLoopAliasesOptions);
                var displayNameStartAlias = AliasesEditorWindow._startLoopAliasesOptions.GetListDisplayName().ToArray();
                selectedStartAll = EditorGUILayout.Popup("Start Aliase", selectedStartAll, displayNameStartAlias);
                var guidValue = currentElemFromArraySelected.FindPropertyRelative("guid").intValue;
                if (selectedStartAll == 0 || AliasUtilityEditor.GetIndexFrom(guidValue, AliasesEditorWindow._startLoopAliasesOptions ) == selectedStartAll)
                    startProp.intValue = 0;
                else
                    startProp.intValue = AliasesEditorWindow._startLoopAliasesOptions[selectedStartAll];
                
                
                
                var endProp = currentElemFromArraySelected.FindPropertyRelative("endAliase");
                int selectedendAll = AliasUtilityEditor.GetIndexFrom(endProp.intValue, AliasesEditorWindow._endLoopAliasesOptions);
                selectedendAll = EditorGUILayout.Popup("End Aliase", selectedendAll, AliasesEditorWindow._endLoopAliasesOptions.GetListDisplayName().ToArray());
                if (selectedendAll == 0 || AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindPropertyRelative("guid").intValue, AliasesEditorWindow._endLoopAliasesOptions ) == selectedendAll)
                    endProp.intValue = 0;
                else
                    endProp.intValue = AliasesEditorWindow._endLoopAliasesOptions[selectedendAll];
            
                EditorGUILayout.EndToggleGroup();
            }
    
            
            SerializedProperty minPitch = currentElemFromArraySelected.FindPropertyRelative("minPitch");
            SerializedProperty maxPitch = currentElemFromArraySelected.FindPropertyRelative("maxPitch");
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
    
                SerializedProperty minDistance = currentElemFromArraySelected.FindPropertyRelative("MinDistance");
                SerializedProperty maxDistance = currentElemFromArraySelected.FindPropertyRelative("MaxDistance");
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
            if (currentElemFromArraySelected.FindPropertyRelative("soundType").enumValueIndex == (int)SoundType.Root)
            {
                DrawField("Text", true);
                DrawField("customDuration", true);
            }
    
            DrawField("isInit", true);
            DrawField("isPlaceholder", true);
           
            
            if (currentElemFromArraySelected.FindPropertyRelative("soundType").enumValueIndex != (int)SoundType.Surface)
            {
                //#if XMATERIAL
                
                
                
                
                SerializedProperty sPUseSurface = currentElemFromArraySelected.FindPropertyRelative("UseSurfaceDetection");
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
                                var aliasSurface = currentElemFromArraySelected.FindPropertyRelative("surfacesAlias");
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
        
                                int selectedSurface = AliasUtilityEditor.GetIndexFrom(guidAliasSurface.intValue, AliasesEditorWindow._surfaceAliasesOptions);
                                selectedSurface = EditorGUILayout.Popup("Alias", selectedSurface, AliasesEditorWindow._surfaceAliasesOptions.GetListDisplayName().ToArray());
                                if (selectedSurface == 0 ||
                                    AliasUtilityEditor.GetIndexFrom(currentElemFromArraySelected.FindPropertyRelative("guid").intValue,
                                        AliasesEditorWindow._surfaceAliasesOptions) == selectedSurface)
                                    guidAliasSurface.intValue = 0;
                                else
                                    guidAliasSurface.intValue = AliasesEditorWindow._surfaceAliasesOptions[selectedSurface];
                            }
                        }
                       
                    }
                }
                EditorGUILayout.EndToggleGroup();
              
               // #endif
            }

            EditorGUILayout.LabelField("GUID",currentElemFromArraySelected.FindPropertyRelative("guid").intValue.ToString());
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
                var tagProp = currentElemFromArraySelected.FindPropertyRelative("Tag");
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