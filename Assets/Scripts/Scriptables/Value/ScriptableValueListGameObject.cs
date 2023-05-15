using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/ListGameObject")]
public class ScriptableValueListGameObject : ScriptableValue<HashSet<GameObject>>
{
    public override HashSet<GameObject> Value { get => base.Value; set => base.Value = value; }
}
