using System;
using UnityEngine;

namespace AudioAliase
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AliaseAttribute : PropertyAttribute { }
    
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AliasEditorAttribute : PropertyAttribute { }

}