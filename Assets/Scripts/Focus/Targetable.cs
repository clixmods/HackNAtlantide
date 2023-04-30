using System;
using UnityEngine;


    public class Targetable : MonoBehaviour , ITargetable
    {
        
        public event EventHandler OnTargeted;
        public event EventHandler OnUntargeted;
        
        [SerializeField] private Transform targeter;
        [SerializeField] private bool useDistance;
        [SerializeField] private float maxDistanceWithTargeter = 50f;
        [SerializeField] private bool canBeTarget;
        
        // Material block 
        private MaterialPropertyBlock[] _propBlocks;
        private Renderer _renderer;
        private static readonly int Amount = Shader.PropertyToID("_Outline");
        
        public bool CanBeTarget
        {
            get
            {
                if (canBeTarget)
                {
                    if (useDistance && targeter != null)
                    {
                        if (Vector3.Distance(targeter.position, transform.position) < maxDistanceWithTargeter)
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

        private void Awake()
        {
            if (targeter == null)
            {
                targeter = PlayerInstanceScriptableObject.Player.transform;
            }
            // Setup Material property block
            _renderer = GetComponentInChildren<Renderer>();
            _propBlocks = new MaterialPropertyBlock[_renderer.sharedMaterials.Length];
            for (int i = 0; i < _propBlocks.Length; i++)
            {
                _propBlocks[i] = new MaterialPropertyBlock();
            }
        }
        
        public void OnTarget()
        {
            OnTargeted?.Invoke(this,null);
            SetFloat(true);
            Debug.Log("Object is target",gameObject);
        }

        public void OnUntarget()
        {
            OnUntargeted?.Invoke(this,null);
            SetFloat(false);
            Debug.Log("Object is untarget", gameObject);
        }

   

        private void SetFloat(bool boolean)
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
