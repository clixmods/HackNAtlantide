using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/Vector3")]
public class ScriptableValueVector3 : ScriptableValue<Vector3>
{
    public override Vector3 Value { get => base.Value; set => base.Value = value; }
}
