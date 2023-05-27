using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TriggerRattleSphere : TriggerRattle
{
    protected override void Init()
    {
        GetComponent<SphereCollider>().radius *= 2;
        if(MeshFilter.sharedMesh != Resources.GetBuiltinResource<Mesh>("Sphere.fbx") )
            MeshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
       
    }
    #if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Trigger/Custom/Trigger Rattle Sphere", false, 1)]
    static void CreateTriggerSphere(MenuCommand menuCommand)
    {
        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            // Create a custom game object
            GameObject triggerGameObject = new GameObject("Trigger Rattle Sphere Sound : undefined");
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
            var trigger = triggerGameObject.AddComponent<TriggerRattleSphere>();
        }
                 
    }
#endif
}
