using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMaterial;

public class GetSurfaceType : MonoBehaviour
{
    // Update is called once per frame
    public string SphereCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1))
        {
            var material = hit.collider.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0];
            return XMaterialsData.GetSurfaceType(material);
        }

        return null;
    }
}
