using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMaterial;

public class FXWalkHandler : MonoBehaviour
{
    [Serializable]
    struct FXWalkSurfaceData
    {
        public string SurfaceName;
        public ParticleSystem FxParticleSystem;
    }

    [SerializeField] private FXWalkSurfaceData[] fxWalkSurfaceData;
    [SerializeField] private ParticleSystem defaultFXToPlay;

    [Header("Raycast Settings")] 
    [SerializeField] private float maxDistance = 0.38f;
    [SerializeField] private Vector3 direction = new Vector3(0,-1,0);
    [SerializeField] private Vector3 offset = new Vector3(0,4.69f,0);
    
    public void PlayFXWalk()
    {
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask("Default");
        if (Physics.Raycast(transform.position + transform.up + offset, direction, out hit, maxDistance, layer_mask,
                QueryTriggerInteraction.Collide) &&
            hit.collider.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
        {
            var material = mesh.sharedMaterials[0];
            var surface = XMaterialsData.GetSurfaceType(material);
            for (int i = 0; i < fxWalkSurfaceData.Length; i++)
            {
                if (!string.IsNullOrEmpty(surface) && surface.Contains(fxWalkSurfaceData[i].SurfaceName))
                {
                    fxWalkSurfaceData[i].FxParticleSystem.Emit(1);
                    return;
                }
            }
            
        }
        defaultFXToPlay.Emit(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + transform.up + offset, transform.position + transform.up + offset + direction * maxDistance);
    }
}