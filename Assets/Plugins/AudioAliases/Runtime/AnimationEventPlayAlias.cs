using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;

public class AnimationEventPlayAlias : MonoBehaviour
{
     public void PlayAlias(Alias aliasID)
     {
        // if(aliasID is Alias alias)
            transform.PlaySoundAtPosition(aliasID);
     }
    
}
