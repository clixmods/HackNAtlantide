using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public static class DataPersistentUtility 
{
    public static string ExtractAssetName(ScriptableObject item)
    {
        string itemName = item.ToString();
        int startIndex = 0; 
        int endIndex = itemName.IndexOf(" (", StringComparison.Ordinal) - 1; 
        int length = endIndex  + 1;
        string finalName = itemName.Substring(startIndex, length);
         
        return finalName;
    }
    public static T GetAssetFromResources<T>(string assetName) where T : Object
    {
        if (string.IsNullOrEmpty(assetName))
        {
            return null;
        }
        var asset = Resources.Load<T>(assetName);
        if (asset == null)
        {
            Debug.LogError($"Asset {assetName} is not found, you need to create the asset or to add it in a Resources Folder");
        }
        return asset;
    }
    public static int GenerateID()
    {
        return Guid.NewGuid().GetHashCode();
    }
    public static bool TryGetSaveInterface(this ScriptableObject scriptableObject,out ISave save)
    {
        save = null;
        var interfacesOnObject = scriptableObject.GetType().GetInterfaces();
        if (interfacesOnObject.Contains(typeof(ISave)))
        {
            save = (ISave) scriptableObject;
            return true;
        }

        return false;
    }
}