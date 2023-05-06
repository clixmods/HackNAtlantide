using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addtocombo : MonoBehaviour
{
    [SerializeField] AttackSO attack;
    
   private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCombat>();
        if(player != null)
        {
            // player.combo.Add(attack);
            Destroy(gameObject);
        }
    }
}