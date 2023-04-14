using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace Zone
{
    public class ZoneVolumeCamera
    {
        [SerializeField] private GameObject prefab;
        #if UNITY_EDITOR
                // Add a menu item to create custom GameObjects.
                // Priority 10 ensures it is grouped with the other menu items of the same kind
                // and propagated to the hierarchy dropdown and hierarchy context menus.
                [MenuItem("GameObject/Trigger/Zone Volume with Camera", false, 1)]
                static void CreateZoneCameraVolume(MenuCommand menuCommand)
                {
                    // Create a custom game object
                    GameObject zoneVolumeGameObject = new GameObject("Zone Volume with camera");
                    GameObjectUtility.SetParentAndAlign(zoneVolumeGameObject, menuCommand.context as GameObject);
                    // Register the creation in the undo system
                    Undo.RegisterCreatedObjectUndo(zoneVolumeGameObject, "Create " + zoneVolumeGameObject.name);
                    Selection.activeObject = zoneVolumeGameObject;
                    // Create Virtual Camera
                    GameObject cameraVirtual = new GameObject("Camera Virtual");
                    GameObjectUtility.SetParentAndAlign(cameraVirtual, zoneVolumeGameObject );
                    // Register the creation in the undo system
                    Undo.RegisterCreatedObjectUndo(cameraVirtual, "Create " + cameraVirtual.name);
                    Selection.activeObject = cameraVirtual;
                    cameraVirtual.AddComponent<CinemachineVirtualCamera>();
                    var view = SceneView.lastActiveSceneView;
                    if (view != null)
                    {
                        cameraVirtual.transform.position = view.camera.transform.position;
                        cameraVirtual.transform.rotation =  view.camera.transform.rotation;
                    }
                    var zoneVolume = zoneVolumeGameObject.AddComponent<CinemachineCameraVirtualTransition>();
                    
                 
                }
        #endif
    }
}