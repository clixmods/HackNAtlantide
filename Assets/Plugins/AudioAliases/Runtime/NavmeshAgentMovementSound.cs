using System;
using UnityEngine;
using UnityEngine.AI;

namespace AudioAliase
{
  
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavmeshAgentMovementSound : MonoBehaviour
    {
        [Serializable]
        struct MovementLength
        {
             public Alias aliasName;
            public float movementLenghtToPlaySound;
            public float velocityMagnitudeCondition;

            public MovementLength(Alias aliasName,float movementLenghtToPlaySound, float velocityMagnitude)
            {
                this.aliasName = aliasName;
                this.movementLenghtToPlaySound = movementLenghtToPlaySound;
                this.velocityMagnitudeCondition = velocityMagnitude;
            }
        }
        [SerializeField]
        private MovementLength[] _movementLengths; 
        private NavMeshAgent _navMeshAgent;
        private float _currentLength = 0;
        [SerializeField] private bool onlyHighest;
        [SerializeField] private Vector3 offsetToPlaySound;
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            _currentLength += _navMeshAgent.velocity.magnitude;
            for (int i = _movementLengths.Length-1; i >= 0; i--)
            {
                if (_navMeshAgent.velocity.magnitude <= _movementLengths[i].velocityMagnitudeCondition)
                    continue;
                
                if (_currentLength >= _movementLengths[i].movementLenghtToPlaySound)
                {
                    _currentLength = 0;
                    AudioManager.PlaySoundAtPosition( _movementLengths[i].aliasName,transform.position+offsetToPlaySound);
                    if (onlyHighest)
                        break;
                }
            }
        }
    }
}