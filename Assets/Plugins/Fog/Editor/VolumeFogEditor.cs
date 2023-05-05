using UnityEditor;

namespace Plugins.Fog.Editor
{
    [CustomEditor(typeof(FogTransition), true)] 
    public class VolumeFogEditor : UnityEditor.Editor
    {
        private SerializedProperty _serializedProperty;
        private bool _fogSettingsIsFolded;
        private const string PropertyName = "fogSetting";
        private FogTransition myTarget;
        private UnityEditor.Editor _editorfogSettings;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _serializedProperty = serializedObject.FindProperty(PropertyName);
            if (_serializedProperty != null)
            {
                myTarget = (FogTransition) target;
                // Draw ScriptableObject in the inspector
                if(_serializedProperty != null && _serializedProperty.objectReferenceValue != null)
                {
                    _fogSettingsIsFolded = EditorGUILayout.InspectorTitlebar(_fogSettingsIsFolded, _serializedProperty.objectReferenceValue);
                    if (_fogSettingsIsFolded)
                    {
                        CreateCachedEditor(_serializedProperty.objectReferenceValue, null, ref _editorfogSettings);
                        EditorGUI.indentLevel++;
                        _editorfogSettings.OnInspectorGUI();
                    }
                }
            }
        }
    }
}
