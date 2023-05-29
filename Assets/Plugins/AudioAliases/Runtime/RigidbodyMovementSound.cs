using System;
using UnityEngine;

namespace AudioAliase
{
    /// <summary>
    /// Data structure representing a movement length to trigger a sound.
    /// </summary>
    [Serializable]
    struct MovementLength
    {
        public Alias aliasName; // Alias for the sound name to play
        public float movementLenghtToPlaySound; // Movement length required to trigger the sound
        public float velocityMagnitudeCondition; // Velocity magnitude condition to trigger the sound

        /// <summary>
        /// Constructor to initialize a new instance of the MovementLength structure.
        /// </summary>
        /// <param name="aliasName">Alias for the sound name to play.</param>
        /// <param name="movementLenghtToPlaySound">Movement length required to trigger the sound.</param>
        /// <param name="velocityMagnitude">Velocity magnitude condition to trigger the sound.</param>
        public MovementLength(Alias aliasName, float movementLenghtToPlaySound, float velocityMagnitude)
        {
            this.aliasName = aliasName;
            this.movementLenghtToPlaySound = movementLenghtToPlaySound;
            this.velocityMagnitudeCondition = velocityMagnitude;
        }
    }

    /// <summary>
    /// Component attached to a GameObject to play sounds based on the movement length of a Rigidbody.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovementSound : MonoBehaviour
    {
        [SerializeField]
        private MovementLength[] _movementLengths; // Array of movement lengths to trigger the sounds
        private Rigidbody _rigidbody;
        private float _currentLength = 0;
        [SerializeField] private bool onlyHighest; // Flag to play only the sound with the highest movement length
        [SerializeField] private Vector3 offsetToPlaySound; // Offset for the position of the sound to play

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _currentLength += _rigidbody.velocity.magnitude;
            for (int i = _movementLengths.Length - 1; i >= 0; i--)
            {
                if (_rigidbody.velocity.magnitude <= _movementLengths[i].velocityMagnitudeCondition)
                    continue;

                if (_currentLength >= _movementLengths[i].movementLenghtToPlaySound)
                {
                    _currentLength = 0;
                    AudioManager.PlaySoundAtPosition(_movementLengths[i].aliasName, transform.position + offsetToPlaySound);
                    if (onlyHighest)
                        break;
                }
            }
        }
    }
}
