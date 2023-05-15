using System;
using UnityEngine;

namespace Attack
{
    public class AttackDamageableEventArgs : EventArgs
    {  
        // class members 
        public IDamageable idamageable;
    }
    /// <summary>
    /// Class to manage attack collision
    /// </summary>
    public interface IAttackCollider
    {
        public GameObject gameObject { get; }
        /// <summary>
        /// Events when the collider hit a damageable
        /// </summary>
        event EventHandler OnCollideWithIDamageable;
        /// <summary>
        /// IAttackCollider component is enabled or not ?
        /// </summary>
        bool enabled { get; set; }
    }
}