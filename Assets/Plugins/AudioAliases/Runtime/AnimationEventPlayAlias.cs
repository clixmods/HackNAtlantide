using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;

public class AnimationEventPlayAlias : MonoBehaviour
{
     public void PlayAlias(int aliasID)
     {
         transform.PlaySoundAtPosition(aliasID);
     }
}
