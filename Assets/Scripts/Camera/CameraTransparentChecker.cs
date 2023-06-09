using UnityEngine;


    public class CameraTransparentChecker : MonoBehaviour
    {
       
    void FixedUpdate()
    {
        //MouseToWorldPosition();
        GameObjectToWorldPosition(PlayerInstanceScriptableObject.Player);
    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    private Vector3 MouseToWorldPosition()
    {
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
        Ray ray = CameraUtility.Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] results = new RaycastHit[8];
        var size = Physics.RaycastNonAlloc(ray, results);
            foreach(RaycastHit hit in results)
            {
                Transform objectTouched = hit.collider.transform;            
                if(objectTouched.TryGetComponent<MeshTransparent>(out MeshTransparent oof))
                {
                    oof.IsHide = true;        
                }
                Hitpoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                #if UNITY_EDITOR
                Debug.DrawLine(CameraUtility.Camera.transform.position, Hitpoint, Color.blue, 0.5f);
                #endif
            }

        return Hitpoint;
    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    private Vector3 GameObjectToWorldPosition(GameObject objectTarget)
    {
        Vector3 Hitpoint = Vector3.zero;
        Vector3 cameraPosition = CameraUtility.Camera.transform.position;
        Vector3 rayDirection = objectTarget.transform.position - cameraPosition;
        // On trace un rayon avec la mousePosition de la souris
        if (Physics.Raycast(cameraPosition,  rayDirection ,  out RaycastHit RayHit, Mathf.Infinity))
        {
            Transform objectTouched = RayHit.collider.transform; // L'object toucher par le raycast
            // On verifie que le parent de l'objet n'est pas le transform de cette class
            // Si il a un autre parent, ca veut dire qu'on a toucher un mesh d'un prefab
            // Il faut donc tout selectionner pour eviter davoir des mesh transparent bizarre
            if(objectTouched.TryGetComponent<MeshTransparent>(out var meshTransparentWatcher))
            {
                meshTransparentWatcher.IsHide = true;        
            }
            Hitpoint = new Vector3(RayHit.point.x, RayHit.point.y, RayHit.point.z);
            #if UNITY_EDITOR
                Debug.DrawLine(cameraPosition, RayHit.collider.transform.position, Color.blue, 0.5f);
            #endif
        }

        return Hitpoint;
    }
}
