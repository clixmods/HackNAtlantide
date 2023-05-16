using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    running,
    dashing,
    dashingAttack,
    idle
}

[CreateAssetMenu(menuName = "Data/PlayerMovementState")]
public class PlayerMovementStateScriptableObject : ScriptableObject
{
    [SerializeField] private MovementState _movementState;

    public MovementState MovementState { get { return _movementState; } set => _movementState = value; }
}
