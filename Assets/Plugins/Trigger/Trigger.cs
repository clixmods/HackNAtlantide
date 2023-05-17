using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class Trigger : MonoBehaviour
{
     [HideInInspector] [SerializeField] private Material materialTrigger;
     private MeshRenderer _meshRenderer;
     protected MeshFilter MeshFilter;
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
     #region MonoBehaviour

     private void Awake()
     {
          SetupCollider();
          GetMeshComponents();
          // Hide renderer in play mode
          _meshRenderer.enabled = false;
     }
     private void OnValidate()
     {
          SetupCollider();
          GetMeshComponents();
          _meshRenderer.material = materialTrigger;
          if(gameObject.layer != 9)
               gameObject.layer = 9;
          
          if (MeshFilter.hideFlags != HideFlags.HideInInspector)
          {
               Init();
               MeshFilter.hideFlags = HideFlags.HideInInspector;
               _meshRenderer.hideFlags = HideFlags.HideInInspector;
          }
     }

     #endregion
     #region Methods

     private void GetMeshComponents()
     {
          MeshFilter = GetComponent<MeshFilter>();
          _meshRenderer = GetComponent<MeshRenderer>();
     }
     private void SetupCollider()
     {
          _collider = GetComponent<Collider>();
          _collider.isTrigger = true;
          _collider.hideFlags = HideFlags.HideInInspector;
     }

     protected abstract void Init();
     protected virtual void TriggerEnter(Collider other) { }
     protected virtual void TriggerExit(Collider other) { }
     private bool IsInteractable(GameObject gameObject)
     {
          return interactWithLayers == (interactWithLayers | (1 << gameObject.layer));
     }

     #endregion
     #region Trigger Event

     private void OnTriggerEnter(Collider other)
     {
          if (IsInteractable(other.gameObject) )
          {
               EventOnTriggerEnter?.Invoke();
               TriggerEnter(other);
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
               EventOnTriggerEnd?.Invoke();
               TriggerExit(other);
               if (disableAfterOnTriggerExit)
               {
                    gameObject.SetActive(false);
               }
          }
     }

     #endregion
}
