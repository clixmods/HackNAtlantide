#define XMATERIAL

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using XEditor.Editor;
#endif

namespace XMaterial
{
    public class XMaterialsData : ScriptableObject
    {
        public List<XMaterial> xMaterials;
        public static Dictionary<Material, XMaterial> XMaterialsDictionary = new Dictionary<Material, XMaterial>();
        public string[] surfaceTypeNames = new string[]{};
        private static XMaterialsData _instance;
        public static XMaterialsData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<XMaterialsData>("XMaterials");
                }
                return _instance;
            }
        }
        public static string[] SurfaceTypeNames => Instance.surfaceTypeNames;
        private void Awake()
        {
            #if UNITY_EDITOR
                xMaterials = GetXMaterials();
                GenerateSurfaceTypes();
            #endif
            Generate();
        }
        
#if UNITY_EDITOR
        private List<XMaterial> GetXMaterials()
        {
            List<XMaterial> xMaterialsValid = new List<XMaterial>();
            var allxMaterialsInAssets = XEditorUtility.GetAssets<XMaterial>().ToList();
            for (int i = 0; i < allxMaterialsInAssets.Count; i++)
            {
                // Xmaterial is valid ?
                if (allxMaterialsInAssets[i].material != null)
                {
                    xMaterialsValid.Add(allxMaterialsInAssets[i]);
                }
            }

            return xMaterialsValid;
        }

        private void OnValidate()
        {
            #if UNITY_EDITOR
            xMaterials = GetXMaterials();
                GenerateSurfaceTypes();
            #endif
        }

        private void GenerateSurfaceTypes()
        {
            
            var tagsSurfaceType = XEditorUtility.GetTags("MaterialType/", true);
            if (surfaceTypeNames.Length != tagsSurfaceType.Length)
            {
                surfaceTypeNames = tagsSurfaceType;
            }
         
        }
#endif
        private static void Generate()
        {
            for (int i = 0; i < Instance.xMaterials.Count; i++)
            {
                XMaterialsDictionary[Instance.xMaterials[i].material] = Instance.xMaterials[i];
            }
        }
        
        public static string GetSurfaceType(Material material)
        {
            if (XMaterialsDictionary.Count == 0)
            {
                Generate();
            }
            if(XMaterialsDictionary.TryGetValue(material,out XMaterial value))
                return value.SurfaceType;
            
            return null;
        }
    }
}