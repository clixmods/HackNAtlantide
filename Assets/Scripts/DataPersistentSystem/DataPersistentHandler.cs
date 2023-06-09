using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using _2DGame.Scripts.Save;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class DataPersistentHandler : ScriptableObject
{
    private const string ParentFolderName = "data/";
    // TODO : Resource.LoadAll is a better way, need to remove the list in the future to improve the system
    [Tooltip("Contains all scriptableObject compatible with DataPersistentSystem, required to be referenced here.")]
    public List<ScriptableObject> scriptableObjectSaveables;
    public static void AddSaveAssetToHandler(ScriptableObject saveAsset)
    {
#if UNITY_EDITOR
        var assetsGUID = AssetDatabase.FindAssets("DataPersistentHandler");
        var dataPersistentAsset = AssetDatabase.LoadAssetAtPath<DataPersistentHandler>(AssetDatabase.GUIDToAssetPath(assetsGUID[0]));
        if(!dataPersistentAsset.scriptableObjectSaveables.Contains(saveAsset))
            dataPersistentAsset.scriptableObjectSaveables.Add(saveAsset);
#endif
    }
#if UNITY_EDITOR

   
    private void OnValidate()
    {
        ScriptableObject[] scriptableObjects = GetAssets<ScriptableObject>();
        scriptableObjectSaveables = new List<ScriptableObject>();
        for (int i = 0; i < scriptableObjects.Length; i++)
        {
            if (scriptableObjects[i] == null) continue;
 
            var interfacesOnObject = scriptableObjects[i].GetType().GetInterfaces();
            if ( interfacesOnObject.Contains(typeof(ISave)) )
            {
//                Debug.Log($"Component to save found in {scriptableObjects[i].name}");
                scriptableObjectSaveables.Add((ScriptableObject)scriptableObjects[i]);
            }
        }
    }
    public static T[] GetAssets<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        int count = guids.Length;
        T[] a = new T[count];
        for (int i = 0; i < count; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }
#endif

    private void Awake()
    {
        for (int i = 0; i < scriptableObjectSaveables.Count; i++)
        {
            if (scriptableObjectSaveables[i] == null)
            {
                scriptableObjectSaveables.RemoveAt(i);
                Awake();
                break;
            }
                
        }
    }

    [ContextMenu("Save")]
    public void SaveAll()
    {
        var instanceSaveable = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveMonoBehavior>();
        // Instance
        foreach (ISaveMonoBehavior saveInstance in instanceSaveable) 
        {
            Save(saveInstance, saveInstance.SaveID.ToString());
        }
        // Asset
        foreach (var dataPersistent in scriptableObjectSaveables)
        {
            if (dataPersistent.TryGetSaveInterface(out ISave save))
            {
                Save(save, dataPersistent.name);
            }
        }
    }
    [ContextMenu("Load All")]
    public void LoadAll()
    {
        var instancesSaveable = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveMonoBehavior>();
        // Instance
        foreach (var instanceSaveable in instancesSaveable)
        {
            Load(instanceSaveable, instanceSaveable.SaveID.ToString());
        }
        // Asset
        foreach (var dataPersistent in scriptableObjectSaveables)
        {
            if (dataPersistent.TryGetSaveInterface(out ISave save))
            {
                Load(save, dataPersistent.name);
            }
        }
    }
    
    /// <summary>
    /// Reset all scriptables objets to have default value
    /// </summary>
    [ContextMenu("Reset All")]
    public void ResetAll()
    {
        // Asset
        foreach (var dataPersistent in scriptableObjectSaveables)
        {
            if (dataPersistent.TryGetSaveInterface(out ISave save))
            {
                save.OnReset();
            }
        }
    }
    public static void Save(ISave save, string fileName)
    {
        save.OnSave(out var gamedata);
        string dataPath = Path.Combine(Application.persistentDataPath, (ParentFolderName +fileName ));
        string jsonData = JsonUtility.ToJson(gamedata, true);
        byte[] byteData;
        byteData = Encoding.ASCII.GetBytes(jsonData);
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
        }
        File.WriteAllBytes(dataPath, byteData);
    }
    public static void Load(ISave save, string fileName)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, (ParentFolderName + fileName));
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + dataPath);
            return;
        }
        byte[] jsonDataAsBytes = null;
        if (File.Exists(dataPath))
        {
            jsonDataAsBytes = File.ReadAllBytes(dataPath);
            string jsonData;
            jsonData = Encoding.ASCII.GetString(jsonDataAsBytes);
            save.OnLoad(jsonData);
        }
    }
}