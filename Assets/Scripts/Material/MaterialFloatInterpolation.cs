using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MaterialFloatInterpolation : MonoBehaviour
{
    [SerializeField] private string floatName;
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

    [SerializeField] private float timeToActiveEmissive = 1f;
    [SerializeField] private float timeToResetEmissive = 0.5f;
    private int EmissionColorID;
    private StateTransition _stateTransition = StateTransition.Idle;


    private void Awake()
    {
        EmissionColorID = Shader.PropertyToID(floatName);
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

    }

    [ContextMenu("activeEmissive")]
    public void ActiveEmissive()
    {
        ActiveEmissive(50);
    }
    public void ActiveEmissive(float value)
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            Debug.Log(_propBlocks[i].GetVector(EmissionColorID));
            StartCoroutine(LerpColor(_propBlocks[i], i, value, timeToActiveEmissive, StateTransition.IsEnabling));
        }
    }
    [ContextMenu("resetEmissive")]
    public void ResetEmissive()
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            StopAllCoroutines();
            StartCoroutine(LerpColor(_propBlocks[i], i,0, timeToResetEmissive, StateTransition.IsDisabling));
        }
    }

    IEnumerator LerpColor(MaterialPropertyBlock materialPropertyBlock, int index, float floatTarget, float timeTransition, StateTransition stateTransition)
    {
        _stateTransition = stateTransition;
        float timeElapsed = 0;
        meshRenderer.GetPropertyBlock(materialPropertyBlock, index);
        // Assign our new value.
        float initialValue = materialPropertyBlock.GetFloat(EmissionColorID);

        while (timeElapsed < timeTransition)
        {
            if (_stateTransition != stateTransition)
            {
                yield break;
            }
            timeElapsed += Time.deltaTime;
            var t = timeElapsed / timeTransition;
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(materialPropertyBlock, index);
            // Assign our new value.
            materialPropertyBlock.SetFloat(EmissionColorID, Mathf.Lerp(initialValue, floatTarget, t));
            // Apply the edited values to the renderer.
            meshRenderer.SetPropertyBlock(materialPropertyBlock, index);
            yield return null;
        }
        _stateTransition = StateTransition.Idle;
    }


}
