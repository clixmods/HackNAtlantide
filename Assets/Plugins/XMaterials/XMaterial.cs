#define XMATERIAL

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using XEditor.Editor;
#endif

namespace XMaterial
{
    public class XMaterial : ScriptableObject
    {
        public Material material;
        public string SurfaceType;
        public string GlossRange;
        public string Usage;
        
      
        
        
         private void Awake()
         {
             
        }

         public void DeleteAsset()
         {
             if (material == null)
             {
                 
             }
         }

#if UNITY_EDITOR
        public void OnValidate()
        {
            var xMaterialsData = Resources.Load<XMaterialsData>("XMaterials");
            if (xMaterialsData == null)
            {
                CreateXMaterialsData();
                xMaterialsData = Resources.Load<XMaterialsData>("XMaterials");
            }
            if(!xMaterialsData.xMaterials.Contains(this))
                xMaterialsData.xMaterials.Add(this);
            
         
            //XMaterialsDictionary[material] = this;
        }

        void CreateXMaterialsData()
        {
            string path = $"Assets/XMaterials/Resources/XMaterials.asset";
            
            var assetXMaterialData = AssetDatabase.LoadAssetAtPath<XMaterialsData>(path);
            if (!string.IsNullOrEmpty(path) && assetXMaterialData == null )
            {
                AssetDatabase.CreateAsset(CreateInstance<XMaterialsData>(), path);
                AssetDatabase.Refresh();
            }
        }
#endif
       
    }
}
