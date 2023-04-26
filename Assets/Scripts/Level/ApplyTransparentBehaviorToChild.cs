using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ApplyTransparentBehaviorToChild : MonoBehaviour
{
    /// <summary> Material transparent qui sera appliqué à tout les models présent en tant qu'enfant </summary>
    [SerializeField] Material mtlTransparent;
    // Start is called before the first frame update
    void Start()
    {
        // On check chaque enfant du gameObject
        for(int i = 0 ; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            ArrayList materialsFromChildMeshRenderers = new ArrayList();
            ArrayList combineInstanceArrays = new ArrayList();
            // Pour l'enfant itérer, on get tout ces MeshFilter
            Vector3 ogPostion = transform.position;
            MeshFilter[] meshFilters = childTransform.GetComponentsInChildren<MeshFilter>();
            MeshTransparentWatcher meshTransparentWatcher;
            foreach (MeshFilter meshFilter in meshFilters)
            {
                // Verifie si le gameobject est activé
                if(!meshFilter.transform.gameObject.activeSelf)
                    continue;
                if(!meshFilter.mesh.isReadable)
                {
                    Debug.LogWarning($"Attention le mesh {meshFilter.mesh.name} n'est pas modifiable, la transparence ne pourra se faire", meshFilter.gameObject);
                    continue;
                }
                MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                if (!meshRenderer ||
                    !meshFilter.sharedMesh ||
                    meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount)
                {
                    continue;
                }
                for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
                {
                    materialsFromChildMeshRenderers.Add(meshRenderer.sharedMaterials[s]);
                }
            }
            if(meshFilters.Length == 1)
            {
                Debug.Log($"GameObject {meshFilters[0].mesh.name} have one mesh filter", meshFilters[0].gameObject);
                meshTransparentWatcher = meshFilters[0].transform.gameObject.AddComponent<MeshTransparentWatcher>();
                meshFilters[0].transform.gameObject.AddComponent<MeshCollider>();
                meshTransparentWatcher.Init(mtlTransparent);
                continue;
            }
            meshTransparentWatcher = childTransform.gameObject.AddComponent<MeshTransparentWatcher>();
            CombineInstance[] combine;
            combine = new CombineInstance[meshFilters.Length];
            int ii = 0;
            while (ii < meshFilters.Length)
            {
                combine[ii].mesh = meshFilters[ii].sharedMesh;
                
                combine[ii].transform = meshFilters[ii].transform.localToWorldMatrix;
               
                Destroy(meshFilters[ii].gameObject);
               
                ii++;
            }
            
            // On verifie si il a pas déja un mesh filter
            if(childTransform.gameObject.TryGetComponent<MeshFilter>(out MeshFilter amf))
            {
                continue;
            }
            // On combine le mesh
            MeshFilter generatedMeshFilter =  childTransform.gameObject.AddComponent<MeshFilter>();
            generatedMeshFilter.mesh = new Mesh();
            generatedMeshFilter.mesh.CombineMeshes(combine, false ,true);
        
            MeshCollider mc =  childTransform.gameObject.AddComponent<MeshCollider>();
            MeshRenderer mr =  childTransform.gameObject.AddComponent<MeshRenderer>();
           
            // Assign materials
            Material[] materialsArray = materialsFromChildMeshRenderers.ToArray(typeof(Material)) as Material[];
            mr.materials = materialsArray;
            
            childTransform.gameObject.SetActive(true);
            childTransform.position = ogPostion;
            childTransform.rotation = new Quaternion(0,0,0,0);
            childTransform.localScale = Vector3.one;
            meshTransparentWatcher.Init(mtlTransparent);

            int childs = childTransform.childCount;
            for (int iii = childs - 1; iii >= 0; iii--)
            {   
                DestroyImmediate(childTransform.GetChild(iii).gameObject);
            }
             
        }
    }
    /// <summary> Combine une list de mesh et renvoi le nouveau generer </summary>
   
    private Mesh CombineMeshes(List<Mesh> meshes)
    {
        var combine = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }

    /// <summary> La transform en param correspond au mesh toucher /// </summary>
    public void ChangeMaterialOnChild(Transform child)
    {
        if(child.TryGetComponent<MeshTransparentWatcher>(out MeshTransparentWatcher oof))
        {
            oof.IsHide = true;
            return;
        }

        MeshRenderer mr = child.GetComponentInChildren<MeshRenderer>();
        for(int ii = 0 ; ii < mr.sharedMaterials.Length; ii++)
        {
            // On copie car le set réassigne uniquement la table, modifié la table directement ne fait que la copié donc pas appliquer
            Material[] materialsToChange = mr.sharedMaterials;
            materialsToChange[ii] = mtlTransparent;           
            mr.sharedMaterials = materialsToChange;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MouseToWorldPosition();
    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    private Vector3 MouseToWorldPosition()
    {
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit[] RayHits = Physics.RaycastAll(ray) ;
            foreach(RaycastHit hit in RayHits)
            {
                Transform objectTouched = hit.collider.transform;            
                if(objectTouched.TryGetComponent<MeshTransparentWatcher>(out MeshTransparentWatcher oof))
                {
                    oof.IsHide = true;        
                }
                Hitpoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                #if UNITY_EDITOR
                if (Hitpoint != null)
                    Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
                #endif
            }

        return Hitpoint;
    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    private Vector3 GameObjectToWorldPosition(GameObject objectTarget)
    {
        Ray ray;
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
        ray = Camera.main.ViewportPointToRay(objectTarget.transform.position); 
        if (Physics.Raycast(Camera.main.transform.position , objectTarget.transform.position - Camera.main.transform.position,  out RaycastHit RayHit, Mathf.Infinity))
        {
            Transform objectTouched = RayHit.collider.transform; // L'object toucher par le raycast
            // On verifie que le parent de l'objet n'est pas le transform de cette class
            // Si il a un autre parent, ca veut dire qu'on a toucher un mesh d'un prefab
            // Il faut donc tout selectionner pour eviter davoir des mesh transparent bizarre
            if(objectTouched.TryGetComponent<MeshTransparentWatcher>(out MeshTransparentWatcher oof))
            {
                oof.IsHide = true;        
            }
            Hitpoint = new Vector3(RayHit.point.x, RayHit.point.y, RayHit.point.z);
            #if UNITY_EDITOR
                Debug.DrawLine(Camera.main.transform.position, RayHit.collider.transform.position, Color.blue, 0.5f);
            #endif
        }

        return Hitpoint;
    }

    
}