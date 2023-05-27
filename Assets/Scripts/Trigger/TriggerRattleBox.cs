﻿using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerRattleBox : TriggerRattle
{
    protected override void Init()
    {
        if(MeshFilter.sharedMesh != Resources.GetBuiltinResource<Mesh>("Cube.fbx") )
            MeshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Custom/Trigger Rattle Box", false, 1)]
    static void CreateTriggerBox(MenuCommand menuCommand)
    {
        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            // Create a custom game object
            GameObject triggerGameObject = new GameObject("Trigger Rattle Box Sound : undefined");
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
            var triggerBox = triggerGameObject.AddComponent<TriggerRattleBox>();
        }
                 
    }
#endif
}
