using System;
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
            MeshTransparent meshTransparent;
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
                meshTransparent = meshFilters[0].transform.gameObject.AddComponent<MeshTransparent>();
                meshFilters[0].transform.gameObject.AddComponent<MeshCollider>();
                meshTransparent.Init(mtlTransparent);
                continue;
            }
            meshTransparent = childTransform.gameObject.AddComponent<MeshTransparent>();
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
            meshTransparent.Init(mtlTransparent);

            int childs = childTransform.childCount;
            for (int iii = childs - 1; iii >= 0; iii--)
            {   
                DestroyImmediate(childTransform.GetChild(iii).gameObject);
            }
             
        }
    }
    /// <summary>
    /// Combine a list of mesh into one mesh
    /// </summary>
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
        if(child.TryGetComponent<MeshTransparent>(out MeshTransparent oof))
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

    

    
}