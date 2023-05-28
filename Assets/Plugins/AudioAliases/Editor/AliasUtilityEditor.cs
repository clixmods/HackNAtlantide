using System;
using System.Collections.Generic;
using AudioAliase;

namespace Audio.Editor
{
    public static class AliasUtilityEditor
    {
        public static List<string> GetListNameAlias(this Dictionary<int, Alias> oof)
        {
            List<string> myList = new List<string>();
            
            foreach(KeyValuePair<int, Alias> entry in oof) 
            {
                myList.Add(entry.Value.name);
            }

            return myList;
        }
        
        public static List<string> GetListDisplayName(this List<int> list, int elementToHide = -1)
        {
            List<string> myList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {

                if (AliasesEditorWindow.Dictionary.TryGetValue(list[i], out Alias value))
                {
                    // if (elementToHide > -1 && elementToHide == value.GUID)
                    // {
                    //     continue;
                    // }
                    myList.Add(value.name);
                }
                    
                else
                {
                    myList.Add("None");
                }
            }
            return myList;
        }
        
        public static int GetIndexFrom(int guid, List<int> array)
        {
            int length = array.Count;
            if (guid == 0)
                return 0;
                
            for (int i = 0; i < length; i++)
            {
                if (array[i] == guid)
                    return i;
            }
            return 0;
        }
        public static int GenerateID()
        {
            return Guid.NewGuid().GetHashCode();
        }
    }

}