using UnityEditor;
using UnityEngine;

namespace XMaterial.Editor
{
    public class AddTagWindow : EditorWindow
    {
        string tagname = "";
        string layername = "";
        private XMaterialEditor _previousWindow;
        public void OnGUI()
        {
            tagname = EditorGUILayout.TextField("", tagname);
            if (tagname == "" || tagname == null)
            {
                GUI.enabled = false;
            }
            else
            {
                GUI.enabled = true;
            }
            if (GUILayout.Button("Create Tag"))
            {
                UnityEditorInternal.InternalEditorUtility.AddTag($"MaterialType/{tagname}");
                Close();
            }
            if (GUILayout.Button("Remove Tag"))
            {
              //  UnityEditorInternal.InternalEditorUtility.RemoveTag();
            }

        }
    }


}