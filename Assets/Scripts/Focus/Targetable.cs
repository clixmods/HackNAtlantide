using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Targetable : MonoBehaviour, ITargetable
{
    public event EventHandler OnTargeted;
    public event EventHandler OnUntargeted;
    [SerializeField] private Transform targeter;
    [SerializeField] private Transform pivot;
    [SerializeField] private bool useDistance;
    [SerializeField] private float maxDistanceWithTargeter = 50f;

    [FormerlySerializedAs("canBeTarget")] [SerializeField] private bool isTargetable;
    [Header("Height")] 
    [SerializeField] private bool checkHeight;
    [SerializeField] private float heightTolerance = 10f;
    // Material block 
    private MaterialPropertyBlock[] _propBlocks;
    private Renderer _renderer;
    private static readonly int Amount = Shader.PropertyToID("_Outline");

    [SerializeField] GameObject _decalTarget;

    public bool CanBeTarget
    {
   
        get
        {
            if (isTargetable)
            {
                if (targeter != null)
                {
                    if (checkHeight)
                    {
                        if (GetHeightWithTargeter() >= heightTolerance)
                        {
                            return false;
                        }
                    }
                    if (useDistance)
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
                }
               

                return isTargetable;
            }

            return false;
        }
    }

    private float GetHeightWithTargeter()
    {
        return Mathf.Abs(transform.position.y - targeter.position.y);
    }

    public Transform targetableTransform
    {
        get
        {
            try
            {
                if (pivot != null)
                    return pivot;

                if (transform != null)
                    return transform;
            }
            catch
            {
                return null;
            }


            return null;
        }
    }

    #region MonoBehaviour

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
    private void OnDestroy()
    {
        ITargetable.Targetables.Remove(this);
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (targeter == null) return;
        
            Color colorLine = Color.red;
            if (CanBeTarget)
            {
                colorLine = Color.green;
            }
            Debug.DrawLine(transform.position, targeter.transform.position, colorLine, 0.001f, true);
            GUIStyle style = new GUIStyle();
            style.normal.textColor = colorLine; 
        
            Handles.Label(transform.position , GetHeightWithTargeter().ToString() , style );
    }
    #endif

    #endregion
    public void OnTarget()
    {
        OnTargeted?.Invoke(this, null);
        SetFloat(true);
        _decalTarget.SetActive(true);
    }

    public void OnUntarget()
    {
        OnUntargeted?.Invoke(this, null);
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
                _renderer.GetPropertyBlock(_propBlocks[i], i);
                // Assign our new value.
                _propBlocks[i].SetInt(Amount, boolean ? 1 : 0);
                // Apply the edited values to the renderer.
                _renderer.SetPropertyBlock(_propBlocks[i], i);
            }
        }
    }

   
}