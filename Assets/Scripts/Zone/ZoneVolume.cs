#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]

[AddComponentMenu("#Survival Game/Gameplay Elements/Zone Volume")]
public class ZoneVolume : MonoBehaviour
{
    /// <summary>
    /// Event when the volume is triggered by Enter
    /// </summary>
    public UnityEvent EventOnTriggerEnter;
    /// <summary>
    /// Event when the volume is triggered by Exit
    /// </summary>
    public UnityEvent EventOnTriggerEnd;
    private BoxCollider _boxCollider;
    [SerializeField] private bool _disableAfterOnTriggerEnter;
    [SerializeField] private bool _disableAfterOnTriggerExit;
    [SerializeField] private LayerMask _layerMaskWithInteract;
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInteractable(other.gameObject) )
        {
            Debug.Log("Trigger Enter by player");
            EventOnTriggerEnter?.Invoke();
            
            if (_disableAfterOnTriggerEnter)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsInteractable(other.gameObject) )
        {
            Debug.Log("Trigger Exit by player");   
         
            EventOnTriggerEnd?.Invoke();
            
            if (_disableAfterOnTriggerExit)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private bool IsInteractable(GameObject gameObject)
    {
        return _layerMaskWithInteract == (_layerMaskWithInteract | (1 << gameObject.layer));
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Zone Volume", false, 1)]
    static void CreateZoneVolume(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Zone Volume");
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<ZoneVolume>();
    }
#endif

}
