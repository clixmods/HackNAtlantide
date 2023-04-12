using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using XEditor.Editor;

namespace XMaterial.Editor
{
    [CustomEditor(typeof (Material))]
public class XMaterialEditor : MaterialEditor
{
    private XMaterial _xMaterial;
    private static string[] tagsMaterialsType;
    public override void OnEnable()
    {
        UpdateTags();
        var xMaterials = XEditorUtility.GetAssetsFromName<XMaterial>(target.name);
        if (xMaterials.Length != 0)
        {
            _xMaterial = xMaterials[0];
            _xMaterial.material = (Material)target;
        }
        base.OnEnable();
    }

    public static void UpdateTags()
    { 
        tagsMaterialsType = XEditorUtility.GetTags("MaterialType/");
    }

    public override void OnInspectorGUI()
    {
        if (_xMaterial != null)
        {
            var serialize = new SerializedObject(_xMaterial);
            DrawXMaterialProperty(serialize, "SurfaceType");
            DrawXMaterialProperty(serialize, "GlossRange");
            DrawXMaterialProperty(serialize, "Usage");
            serialize.ApplyModifiedProperties();
        }
        else
        {
            if (GUILayout.Button("Generate XMaterial properties"))
            {
                XMaterial mySO = ScriptableObject.CreateInstance<XMaterial>();
                string assetPath = AssetDatabase.GetAssetPath(target);
                if (assetPath != String.Empty)
                {
                    string parentFolder = Path.GetDirectoryName(assetPath);
                    Debug.Log(AssetDatabase.GetAssetPath(target));
                    AssetDatabase.CreateAsset(mySO, $"Assets/XMaterials/{target.name}.asset");
                    AssetDatabase.SaveAssets();
                    var xMaterials = XEditorUtility.GetAssetsFromName<XMaterial>(target.name);
                    if (xMaterials.Length != 0)
                    {
                        _xMaterial = xMaterials[0];
                        _xMaterial.material = (Material)target;
                    }
                }
            }
        }
        // Afficher le contenu par défaut de l'inspecteur Unity
        base.OnInspectorGUI();
    }

    private static void DrawXMaterialProperty(SerializedObject serialize, string propertyPath)
    {
        var tagProp = serialize.FindProperty(propertyPath);
        int selectedTag = XEditorUtility.GetIndexFromNameTag(tagsMaterialsType, tagProp.stringValue);
        selectedTag = EditorGUILayout.Popup(propertyPath, selectedTag, tagsMaterialsType);
        if (selectedTag == tagsMaterialsType.Length - 1) // Is Add Tag field
        {
            EditorWindow.GetWindow<AddTagWindow>();
            UpdateTags();
        }
        else
        {
            if (selectedTag == 0)
                tagProp.stringValue = string.Empty;
            else
                tagProp.stringValue = tagsMaterialsType[selectedTag];
        }
    }
}

}