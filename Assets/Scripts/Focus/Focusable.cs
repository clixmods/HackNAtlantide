using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Focusable : MonoBehaviour, IFocusable
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
    [SerializeField] GameObject _decalTarget;

    public bool CanBeFocusable
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
                        
                        if (Vector3.Distance(targeter.position, focusableTransform.position) < maxDistanceWithTargeter)
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

    public Transform focusableTransform
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

    private void Start()
    {
        if (targeter == null)
        {
            targeter = PlayerInstanceScriptableObject.Player.transform;
        }
        IFocusable.Focusables.Add(this);
    }
    private void OnDestroy()
    {
        IFocusable.Focusables.Remove(this);
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (targeter == null) return;
        
            Color colorLine = Color.red;
            if (CanBeFocusable)
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
    public void OnFocus()
    {
        OnTargeted?.Invoke(this, null);
        _decalTarget.SetActive(true);
    }

    public void OnUnfocus()
    {
        OnUntargeted?.Invoke(this, null);
        _decalTarget.SetActive(false);
    }
}