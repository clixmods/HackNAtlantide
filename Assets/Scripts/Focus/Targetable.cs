using System;
using UnityEngine;


    public class Targetable : MonoBehaviour , ITargetable
    {
        public event EventHandler OnTargeted;
        public event EventHandler OnUntargeted;
        [SerializeField] private Transform targeter;
        [SerializeField] private Transform pivot;
        [SerializeField] private bool useDistance;
        [SerializeField] private float maxDistanceWithTargeter = 50f;
        [SerializeField] private bool canBeTarget;
        // Material block 
        private MaterialPropertyBlock[] _propBlocks;
        private Renderer _renderer;
        private static readonly int Amount = Shader.PropertyToID("_Outline");

        [SerializeField] GameObject _decalTarget;
        public bool CanBeTarget
        {
            get
            {
                if (canBeTarget)
                {
                    if (useDistance && targeter != null)
                    {
                        if (Vector3.Distance(targeter.position, targetableTransform.position) < maxDistanceWithTargeter)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return canBeTarget;
                }

                return false;

            }
        }
        public Transform targetableTransform
        {
            get
            {
                try
                {
                    if(pivot != null)
                        return pivot;
                
                    if(transform != null)
                        return transform;
                }
                catch
                {
                    return null;
                }
               

                return null;
            }
        }
        private void Awake()
        {
            if (targeter == null)
            {
                targeter = PlayerInstanceScriptableObject.Player.transform;
            }
            // Setup Material property block
            _renderer = GetComponentInChildren<Renderer>();
            if (_renderer != null)
            {
                _propBlocks = new MaterialPropertyBlock[_renderer.sharedMaterials.Length];
                for (int i = 0; i < _propBlocks.Length; i++)
                {
                    _propBlocks[i] = new MaterialPropertyBlock();
                }
            }
            ITargetable.Targetables.Add(this);
        }
        public void OnTarget()
        {
            OnTargeted?.Invoke(this,null);
            SetFloat(true);
            _decalTarget.SetActive(true);
        }
        public void OnUntarget()
        {
            OnUntargeted?.Invoke(this,null);
            SetFloat(false);
            _decalTarget.SetActive(false);
        }
        private void SetFloat(bool boolean)
        {
            if (_propBlocks != null)
            {
                for (int i = 0; i < _propBlocks.Length; i++)
                {
                    // Get the current value of the material properties in the renderer.
                    _renderer.GetPropertyBlock(_propBlocks[i],i);
                    // Assign our new value.
                    _propBlocks[i].SetInt(Amount, boolean ? 1 : 0   );  
                    // Apply the edited values to the renderer.
                    _renderer.SetPropertyBlock(_propBlocks[i], i );
                }
            }
            
        }

        private void OnDestroy()
        {
            ITargetable.Targetables.Remove(this);
        }
    }
