using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBoxHeal : TriggerBox
{
    [Header("Heal Settings")] 
    public UnityEvent HealUsed;
    public UnityEvent NoHealAvailable;
    [SerializeField] private float healAmount = 1;
    [SerializeField] private float healLimit = 3;
    protected override void TriggerStay(Collider other)
    {
        base.TriggerStay(other);
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (healLimit > 0)
            {
                if (damageable.health < damageable.maxHealth)
                {
                    HealUsed?.Invoke();
                    damageable.AddHealth(healAmount);
                    healLimit--;
                }
            }
            else
            {
                NoHealAvailable?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Custom/Trigger Box Heal", false, 1)]
    static void CreateTriggerBox(MenuCommand menuCommand)
    {
        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            // Create a custom game object
            GameObject triggerGameObject = new GameObject("Box Trigger Heal");
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
            var triggerBox = triggerGameObject.AddComponent<TriggerBoxHeal>();
        }
                 
    }
#endif
}
