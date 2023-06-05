using System;
using System.Collections;
using UnityEngine;

public class EmissiveMaterialHandler : MonoBehaviour
{
    enum StateTransition
    {
        Idle,
        IsEnabling,
        IsDisabling
    }
    private MaterialPropertyBlock[] _propBlocks;
    [SerializeField] private Renderer meshRenderer;
     
    [ColorUsage(true, true)] 
    [SerializeField] private Color emissiveColor;
    [SerializeField] [ColorUsage(true, true)] private Color[] _initialColor;

    [SerializeField] private float timeToActiveEmissive = 1f;
    [SerializeField] private float timeToResetEmissive = 0.5f;
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    private StateTransition _stateTransition = StateTransition.Idle;
    

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
       
    }

    private void Start()
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            // Assign our new value.
            _initialColor[i] = meshRenderer.sharedMaterials[i].GetVector(EmissionColorID);
        }
    }

    public void ActiveEmissive()
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            StartCoroutine(LerpColor(_propBlocks[i] , i , emissiveColor , timeToActiveEmissive, StateTransition.IsEnabling )); 
        } 
    }

    public void ResetEmissive()
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            StartCoroutine(LerpColor(_propBlocks[i] , i , _initialColor[i] , timeToResetEmissive, StateTransition.IsDisabling)); 
        }
    }
    
    IEnumerator LerpColor(MaterialPropertyBlock materialPropertyBlock, int index , Color colorTarget, float timeTransition, StateTransition stateTransition)
    {
        _stateTransition = stateTransition;
        float timeElapsed = 0;
        meshRenderer.GetPropertyBlock(materialPropertyBlock,index);
        // Assign our new value.
        Color initialColor = materialPropertyBlock.GetVector(EmissionColorID);  

        while (timeElapsed < timeTransition)
        {
            if (_stateTransition != stateTransition)
            {
                yield break;
            }
            timeElapsed += Time.deltaTime;
            var t = timeElapsed / timeTransition;
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(materialPropertyBlock,index);
            // Assign our new value.
            materialPropertyBlock.SetVector(EmissionColorID, Vector4.Lerp(initialColor, colorTarget, t));  
            // Apply the edited values to the renderer.
            meshRenderer.SetPropertyBlock(materialPropertyBlock, index );
            yield return null;
        }
        _stateTransition = StateTransition.Idle;
    }
}
