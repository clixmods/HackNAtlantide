using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriggerBoxResetInteractable : TriggerBox
{
    protected override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.ResetTransform();
        }
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Custom/Trigger Box Reset Interactable", false, 1)]
    static void CreateTriggerBox(MenuCommand menuCommand)
    {
        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            // Create a custom game object
            GameObject triggerGameObject = new GameObject("Box Trigger Reset Interactable");
            var raycastHits = Physics.RaycastAll(view.camera.transform.position, view.camera.transform.forward);
            if (raycastHits.Length > 0)
            {
                triggerGameObject.transform.position = raycastHits[^1].point;
            }
            GameObjectUtility.SetParentAndAlign(triggerGameObject, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(triggerGameObject, "Create " + triggerGameObject.name);
            Selection.activeObject = triggerGameObject;
            // Create TriggerBox
            var triggerBox = triggerGameObject.AddComponent<TriggerBoxResetInteractable>();
        }
                 
    }
#endif
}
