#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    [SerializeField] private bool disableAfterOnTriggerEnter;
    [SerializeField] private bool disableAfterOnTriggerExit;
    [SerializeField] private LayerMask layersMaskWithInteract;
    public LayerMask LayersMaskWithInteract
    {
        get
        {
            return layersMaskWithInteract;
        }
        set
        {
            layersMaskWithInteract = value;
        }
    }
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
            Debug.Log("Trigger Exit by player");   
         
            EventOnTriggerEnd?.Invoke();
            
            if (disableAfterOnTriggerExit)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private bool IsInteractable(GameObject gameObject)
    {
        return layersMaskWithInteract == (layersMaskWithInteract | (1 << gameObject.layer));
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
