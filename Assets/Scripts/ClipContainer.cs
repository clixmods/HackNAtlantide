using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipContainer : MonoBehaviour
{
    //[SerializeField] private Material clipMaterial;
    [SerializeField] private bool showFloorIngame = true;
    [SerializeField] private bool showWallIngame = true;
    private void Awake()
    {
        //if (!showFloorIngame)
        {
            foreach (Transform child in transform)
            {
                var childsWithMeshRenderers = child.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer component in childsWithMeshRenderers)
                {
                    if (!showFloorIngame && component.gameObject.layer == 11  )
                    {
                        component.enabled = false;
                    }
                    if (!showWallIngame && component.gameObject.layer == 3 )
                    {
                        component.enabled = false;
                    }
                }
                // if (child.CompareTag("Floor") && child.TryGetComponent<MeshRenderer>( out MeshRenderer meshRendererFloor))
                // {
                //     meshRendererFloor.enabled = false;
                // }
            }
        }

        // if (!showWallIngame)
        // {
        //     foreach (Transform child in transform)
        //     {
        //         if (child.CompareTag("Wall") && child.TryGetComponent<MeshRenderer>( out MeshRenderer meshRendererWall))
        //         {
        //             meshRendererWall.enabled = false;
        //         }
        //     }
        // }
    }

    private void OnValidate()
    {
        // foreach (Transform child in transform)
        // {
        //     child.GetComponentInChildren<MeshRenderer>().sharedMaterial = clipMaterial;
        // }
    }
}
