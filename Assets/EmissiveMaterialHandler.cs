using System;
using System.Collections;
using UnityEngine;

public class EmissiveMaterialHandler : MonoBehaviour
{
    private MaterialPropertyBlock[] _propBlocks;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] [ColorUsage(true, true)] private Color emissiveColor;
    [ColorUsage(true, true)] private Color[] _initialColor;

    [SerializeField] private float timeToActiveEmissive = 1f;
    [SerializeField] private float timeToResetEmissive = 0.5f;
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    enum State
    {
        None,
        Active,
        IsDesactive
    }

    private void Awake()
    {
        // Setup Material property block
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<Renderer>();
        }

        _propBlocks = new MaterialPropertyBlock[meshRenderer.sharedMaterials.Length];
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            _propBlocks[i] = new MaterialPropertyBlock();
        }
        _initialColor = new Color[_propBlocks.Length];
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(_propBlocks[i],i);
            // Assign our new value.
            _initialColor[i] = _propBlocks[i].GetColor(EmissionColorID);
        }
    }

    public void ActiveEmissive()
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            //StopCoroutine(SetColor(_propBlocks[i] , i , _initialColor[i] , timeToResetEmissive)); 
            StartCoroutine(SetColor(_propBlocks[i] , i , emissiveColor , timeToActiveEmissive )); 
        } 
    }

    public void ResetEmissive()
    {
        
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            //
            StartCoroutine(SetColor(_propBlocks[i] , i , _initialColor[i] , timeToResetEmissive)); 
        } 
        
    }
    
    IEnumerator SetColor(MaterialPropertyBlock materialPropertyBlock, int index , Color colorTarget, float timeTransition)
    {
        float timeElapsed = 0;
        meshRenderer.GetPropertyBlock(materialPropertyBlock,index);
        // Assign our new value.
        Color initialColor = materialPropertyBlock.GetColor(EmissionColorID);  
        // Apply the edited values to the renderer.
        meshRenderer.SetPropertyBlock(materialPropertyBlock, index);
        while (timeElapsed < timeTransition)
        {
            timeElapsed += Time.deltaTime;
            var t = timeElapsed / timeTransition;
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(materialPropertyBlock,index);
            // Assign our new value.
            materialPropertyBlock.SetColor(EmissionColorID, Color.Lerp(initialColor, colorTarget, t));  
            // Apply the edited values to the renderer.
            meshRenderer.SetPropertyBlock(materialPropertyBlock, index );
            yield return null;
        }
        
    }
}
