using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace AudioAliase
{
    [CreateAssetMenu(fileName = "new_alias", menuName = "Audio/Alias Loop", order = 0)]
    public class AliasLoop : Alias
    {
        protected override void Init()
        {
            isLooping = true;
            UseSurfaceDetection = false;
        }
    }
}