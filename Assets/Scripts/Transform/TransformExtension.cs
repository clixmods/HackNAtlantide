using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension 
{
    public static void SetParentToNull(this Transform transform)
    {
        transform.parent = null;
    }
}
