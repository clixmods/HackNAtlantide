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
                    
                    var view = SceneView.lastActiveSceneView;
                    if (view != null)
                    {
                        // Create a custom game object
                        GameObject zoneVolumeGameObject = new GameObject("Box Trigger : [Specify something]");
                        var raycastHits = Physics.RaycastAll(view.camera.transform.position, view.camera.transform.forward);
                        if (raycastHits.Length > 0)
                        {
                            zoneVolumeGameObject.transform.position = raycastHits[0].point;
                        }
                        else
                        {
                            zoneVolumeGameObject.transform.position += view.camera.transform.forward * 5;
                        }
                        GameObjectUtility.SetParentAndAlign(zoneVolumeGameObject, menuCommand.context as GameObject);
                        // Register the creation in the undo system
                        Undo.RegisterCreatedObjectUndo(zoneVolumeGameObject, "Create " + zoneVolumeGameObject.name);
                        Selection.activeObject = zoneVolumeGameObject;
                        // Create Virtual Camera
                        GameObject cameraVirtual = new GameObject("Camera Virtual : [Specify something]");
                        //GameObjectUtility.SetParentAndAlign(cameraVirtual, zoneVolumeGameObject );
                        // Register the creation in the undo system
                        Undo.RegisterCreatedObjectUndo(cameraVirtual, "Create " + cameraVirtual.name);
                        Selection.activeObject = cameraVirtual;
                        var cinemachineVirtualCamera = cameraVirtual.AddComponent<CinemachineVirtualCamera>();
                        cinemachineVirtualCamera.m_Lens.FieldOfView = 20;
                        // Apply default setting
                        CinemachineFramingTransposer body = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
                        var framingTransposer = body;
                        framingTransposer.m_CameraDistance = 45;
                        framingTransposer.m_DeadZoneWidth = 0.05f;
                        framingTransposer.m_DeadZoneHeight = 0.05f;
                        
                        
                        cameraVirtual.transform.position = view.camera.transform.position;
                        cameraVirtual.transform.rotation =  view.camera.transform.rotation;
                    
                    
                         var cinemachineCameraVirtualTransition = zoneVolumeGameObject.AddComponent<CinemachineCameraVirtualTransition>();
                         cinemachineCameraVirtualTransition.Init(cinemachineVirtualCamera);
                         //cinemachineVirtualCamera.Follow = GameObject.FindWithTag("Player").transform;
                    }
                 
                }
        #endif
    }
}