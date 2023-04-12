using UnityEditor;
using UnityEngine;

namespace XEditor.Editor
{
    public static class XEditorUtility
    {
        private const string TAG_MNGR_ASSET = "ProjectSettings/TagManager.asset";
        private static int _numberTags;    
        private static string[] _tags;
        // Find every instance of a ScriptableObjects
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

        public static T[] GetAssetsFromName<T>(string name) where T : ScriptableObject
        {
            string searchFilter = $"t:{typeof(T).Name} {name}";
            string[] assetGUIDs = AssetDatabase.FindAssets(searchFilter);
            int count = assetGUIDs.Length;
            T[] a = new T[count];
            for (int i = 0; i < count; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
        
        public static string[] GetTags(string categoryName, bool OnlyValue = false)
        {
            _numberTags = UnityEditorInternal.InternalEditorUtility.tags.Length + 2;
            
            string[] temptagarray = new string[_numberTags];
            int newSize = 0;
            if (!OnlyValue)
            {
                temptagarray[newSize] = "None";
                newSize++;
            }
           
            for (int i = 1; i < _numberTags - 1; i++)
            {
                string tagSelected = UnityEditorInternal.InternalEditorUtility.tags[i - 1];
                if (tagSelected.Contains(categoryName))
                {
                    temptagarray[newSize] = tagSelected;
                    newSize++;
                }
            }

            if (!OnlyValue)
            {
                newSize++;
            }

            string[] tagsAll = new string[newSize];
            if (!OnlyValue)
            {
                temptagarray[newSize - 1] = "Add tag..";
            }

            for (int i = 0; i < newSize; i++)
            {
                tagsAll[i] = temptagarray[i];
            }
            return tagsAll;
        }

        public static int GetIndexFromNameTag(string[] tags ,string tagName)
        {
            string lastString = tags[^1];
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] == tagName)
                    return i;
            }

            return 0;
        }
    }
}