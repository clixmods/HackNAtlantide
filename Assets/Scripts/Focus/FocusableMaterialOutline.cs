using System;
using UnityEngine;

public class FocusableMaterialOutline : MonoBehaviour
{
    private IFocusable _focusable;
    // Material block 
    private MaterialPropertyBlock[] _propBlocks;
    [SerializeField] private Renderer _renderer;
    private static readonly int Amount = Shader.PropertyToID("_Outline");
    private void Awake()
    {
        _focusable = GetComponent<IFocusable>();
        // Setup Material property block
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<Renderer>();
        }
        if (_renderer != null)
        {
            _propBlocks = new MaterialPropertyBlock[_renderer.sharedMaterials.Length];
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                _propBlocks[i] = new MaterialPropertyBlock();
            }
        }
    }

    private void OnEnable()
    {
        _focusable.OnTargeted += FocusableOnOnTargeted;
        _focusable.OnUntargeted += FocusableOnOnUntargeted;
    }

    private void FocusableOnOnUntargeted(object sender, EventArgs e)
    {
        SetFloat(false);
    }

    private void FocusableOnOnTargeted(object sender, EventArgs e)
    {
        SetFloat(true);
    }
    
    private void SetFloat(bool boolean)
    {
        if (_propBlocks != null)
        {
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                // Get the current value of the material properties in the renderer.
                _renderer.GetPropertyBlock(_propBlocks[i], i);
                // Assign our new value.
                _propBlocks[i].SetInt(Amount, boolean ? 1 : 0);
                // Apply the edited values to the renderer.
                _renderer.SetPropertyBlock(_propBlocks[i], i);
            }
        }
    }
}