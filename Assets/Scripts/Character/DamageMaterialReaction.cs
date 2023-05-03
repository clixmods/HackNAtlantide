using System;
using System.Collections;
using UnityEngine;

    public class DamageMaterialReaction : MonoBehaviour
    {
        private IDamageable idamageable;
        private MaterialPropertyBlock[] _propBlocks;
        [SerializeField] private Renderer _meshRenderer;
        private static readonly int Amount = Shader.PropertyToID("_Flick");
        private void Awake()
        {
            idamageable = GetComponentInChildren<IDamageable>();
            idamageable.OnDamage += IdamageableOnOnDamage;
            // Setup Material property block
            _meshRenderer ??= GetComponentInChildren<Renderer>();
            _propBlocks = new MaterialPropertyBlock[_meshRenderer.sharedMaterials.Length];
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                _propBlocks[i] = new MaterialPropertyBlock();
            }
        }
        private void IdamageableOnOnDamage(object sender, EventArgs e)
        {
            StartCoroutine(Flick());
        }
        IEnumerator Flick()
        {
            SetFloat(true);
            yield return new WaitForSeconds(0.5f);
            SetFloat(false);
        }
        private void SetFloat(bool boolean)
        {
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                // Get the current value of the material properties in the renderer.
                _meshRenderer.GetPropertyBlock(_propBlocks[i],i);
                // Assign our new value.
                _propBlocks[i].SetInt(Amount, boolean ? 1 : 0   );  
                // Apply the edited values to the renderer.
                _meshRenderer.SetPropertyBlock(_propBlocks[i], i );
            }
        }
    }




