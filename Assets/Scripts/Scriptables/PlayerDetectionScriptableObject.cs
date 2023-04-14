using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerDetection")]
public class PlayerDetectionScriptableObject : ScriptableObject 
{
    [SerializeField] private float _maxDistance;
    public float MaxDistance { get { return _maxDistance; } }

    Vector3 _playerPosition = Vector3.zero;
    public Vector3 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    public bool IsInRange(Vector3 interactablePosition)
    {
        return (interactablePosition - PlayerPosition).sqrMagnitude < _maxDistance*_maxDistance;
    }

}
