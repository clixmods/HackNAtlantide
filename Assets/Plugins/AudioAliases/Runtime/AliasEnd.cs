using UnityEngine;

namespace AudioAliase
{
    [CreateAssetMenu(fileName = "new_alias", menuName = "Audio/Alias End", order = 1)]
    public class AliasEnd : Alias
    {
        protected override void Init()
        {
            isLooping = false;
            UseSurfaceDetection = false;
        }
    }
}