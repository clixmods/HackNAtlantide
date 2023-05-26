using AudioAliase;
using UnityEditor;
using UnityEngine;

namespace Audio.Editor
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : UnityEditor.Editor
    {
        private bool _foldSO;
        private UnityEditor.Editor _editorSO;
        
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            AudioManager audioManager = (AudioManager) target;
           
            var serializedProperty = serializedObject.FindProperty("_audioManagerData");
            if(serializedProperty.objectReferenceValue == null)
                serializedProperty.objectReferenceValue  = (AudioManagerData) Resources.Load("AudioManager Data") ;
            _foldSO = EditorGUILayout.InspectorTitlebar(_foldSO, serializedProperty.objectReferenceValue);
            if (_foldSO)
            {
                CreateCachedEditor(serializedProperty.objectReferenceValue, null, ref _editorSO);
                EditorGUI.indentLevel++;
                _editorSO.OnInspectorGUI();
            }
        }
    }
}