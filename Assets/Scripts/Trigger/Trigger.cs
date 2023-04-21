using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
     private Collider _collider;
     /// <summary>
     /// Event when the volume is triggered by Enter
     /// </summary>
     public UnityEvent EventOnTriggerEnter;
     /// <summary>
     /// Event when the volume is triggered by Exit
     /// </summary>
     public UnityEvent EventOnTriggerEnd;
     [SerializeField] private bool disableAfterOnTriggerEnter;
     [SerializeField] private bool disableAfterOnTriggerExit;
     [SerializeField] private LayerMask interactWithLayers;

    
     private void OnValidate()
     {
          _collider = GetComponent<Collider>();
          _collider.isTrigger = true;
     }

     private bool IsInteractable(GameObject gameObject)
     {
          return interactWithLayers == (interactWithLayers | (1 << gameObject.layer));
     }
     private void OnTriggerEnter(Collider other)
     {
          if (IsInteractable(other.gameObject) )
          {
               Debug.Log("Trigger Enter by player");
               EventOnTriggerEnter?.Invoke();
            
               if (disableAfterOnTriggerEnter)
               {
                    gameObject.SetActive(false);
               }
          }
     }
     private void OnTriggerExit(Collider other)
     {
          if (IsInteractable(other.gameObject) )
          {
               Debug.Log($"Trigger Exit by {other.gameObject}", gameObject);
               EventOnTriggerEnd?.Invoke();
            
               if (disableAfterOnTriggerExit)
               {
                    gameObject.SetActive(false);
               }
          }
     }
}
