using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class Trigger : MonoBehaviour
{
     [HideInInspector] [SerializeField] private Material materialTrigger;
     private MeshRenderer _meshRenderer;
     protected MeshFilter _meshFilter;
     
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
     private void Awake()
     {
          // Hide renderer in play mode
          _meshRenderer.enabled = false;
     }
     private void OnValidate()
     {
          _collider = GetComponent<Collider>();
          _collider.isTrigger = true;
          _collider.hideFlags = HideFlags.HideInInspector;
          _meshFilter = GetComponent<MeshFilter>();
          _meshRenderer = GetComponent<MeshRenderer>();
          _meshRenderer.material = materialTrigger;
          if (_meshFilter.hideFlags != HideFlags.HideInInspector)
          {
               Init();
               _meshFilter.hideFlags = HideFlags.HideInInspector;
               _meshRenderer.hideFlags = HideFlags.HideInInspector;
          }
     }
     protected abstract void Init();
     protected virtual void TriggerEnter() { }
     protected virtual void TriggerExit() { }
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
               TriggerEnter();
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
               TriggerExit();
               if (disableAfterOnTriggerExit)
               {
                    gameObject.SetActive(false);
               }
          }
     }
}
