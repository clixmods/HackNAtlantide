using AudioAliase;
using UnityEngine;

[CreateAssetMenu(fileName = "new_alias", menuName = "Audio/Alias", order = 0)]
public class AliasBase : Alias
{
    protected override void Init()
    {
        isLooping = false;
    }
}