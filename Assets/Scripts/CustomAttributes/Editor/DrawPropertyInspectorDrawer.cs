using UnityEditor;
using UnityEngine;

namespace CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DrawPropertyInspectorAttribute))]
    public class DrawPropertyInspectorDrawer : PropertyDrawer
    {
        private UnityEditor.Editor _editor;
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);
            _editor.OnInspectorGUI();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 250;
        }
    }
}