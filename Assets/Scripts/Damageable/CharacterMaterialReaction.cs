
    using UnityEngine;

    public class CharacterMaterialReaction : DamageableMaterialReaction
    {
        [SerializeField] private Character _character;
        protected override void Awake()
        {
            _character.OnDamage += IdamageableOnOnDamage;
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
    }
