using System;
using System.Collections;
using System.Collections.Generic;
using AudioAliase;
using UnityEngine;

/// <summary>
/// A script that allows an object to play a rattling sound when triggered or collided with other objects.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Rattle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The alias ID for the rattling sound")]
    private Alias aliasRattle;

    [SerializeField]
    [Tooltip("Determines if the sound should play on trigger enter")]
    private bool playOnEnter = true;

    [SerializeField]
    [Tooltip("Determines if the sound should play on trigger exit")]
    private bool playOnExit;

    /// <summary>
    /// Method that is called when the script is validated in the Unity editor.
    /// </summary>
    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (aliasRattle != null)
        {
            gameObject.name = $"Rattle Sound : {aliasRattle.aliasName}";
        }
        #endif
    }

    /// <summary>
    /// Method that is called when this object enters a trigger collider.
    /// </summary>
    /// <param name="other">The collider of the other object that triggered this event.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (playOnEnter)
            transform.PlaySoundAtPosition(aliasRattle);
    }

    /// <summary>
    /// Method that is called when this object exits a trigger collider.
    /// </summary>
    /// <param name="other">The collider of the other object that triggered this event.</param>
    private void OnTriggerExit(Collider other)
    {
        if (playOnExit)
            transform.PlaySoundAtPosition(aliasRattle);
    }

    /// <summary>
    /// Method that is called when this object collides with another object.
    /// </summary>
    /// <param name="collision">The collision data of the collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (playOnEnter)
            transform.PlaySoundAtPosition(aliasRattle);
    }

    /// <summary>
    /// Method that is called when this object stops colliding with another object.
    /// </summary>
    /// <param name="other">The collision data of the collision event.</param>
    private void OnCollisionExit(Collision other)
    {
        if (playOnExit)
            transform.PlaySoundAtPosition(aliasRattle);
    }
}