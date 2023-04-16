using System;
using System.Collections.Generic;


namespace AudioAliase
{
    public static class AliasUtility
    {
        
        public static int GenerateID()
        {
            return Guid.NewGuid().GetHashCode();
        }
    }
    
}