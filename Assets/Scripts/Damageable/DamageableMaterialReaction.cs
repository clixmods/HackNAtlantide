using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageableMaterialReaction : MonoBehaviour
    {
        protected IDamageable idamageable;
        protected MaterialPropertyBlock[] _propBlocks;
        [FormerlySerializedAs("_meshRenderer")] [SerializeField] protected Renderer meshRenderer;
        private static readonly int Amount = Shader.PropertyToID("_Flick");
        protected virtual void Awake()
        {
            idamageable = GetComponentInChildren<IDamageable>();
            idamageable.OnDamage += IdamageableOnOnDamage;
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
            SetFloat(false);
        }
        protected void IdamageableOnOnDamage(object sender, EventArgs e)
        {
            StartCoroutine(Flick());
        }
        IEnumerator Flick()
        {
            SetFloat(true);
            yield return new WaitForSeconds(0.5f);
            SetFloat(false);
        }
        protected void SetFloat(bool boolean)
        {
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                // Get the current value of the material properties in the renderer.
                meshRenderer.GetPropertyBlock(_propBlocks[i],i);
                // Assign our new value.
                _propBlocks[i].SetInt(Amount, boolean ? 1 : 0   );  
                // Apply the edited values to the renderer.
                meshRenderer.SetPropertyBlock(_propBlocks[i], i );
            }
        }
    }




