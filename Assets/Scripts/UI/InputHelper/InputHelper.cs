using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputHelper : MonoBehaviour
{
    [SerializeField] protected Sprite _iconObject;
    [SerializeField] protected InputActionReference _inputActionReference;
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected float maxDistanceToShow = 45;
    [Tooltip("If is true, the UI Element is not connected with the manipulation of this component")]
    [SerializeField] protected bool _UIElementIsIndependant;
    private UIInputHelper _uiInputHelper;

    public UIInputHelper UIInputHelper => _uiInputHelper;
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _uiInputHelper = UIInputHelper.CreateInputHelper(_prefab, transform, _iconObject , maxDistanceToShow,_inputActionReference);
        
    }

    private void OnDisable()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null && _uiInputHelper.gameObject.activeSelf )
            _uiInputHelper.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null && !_uiInputHelper.gameObject.activeSelf)
            _uiInputHelper.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null)
            Destroy(_uiInputHelper.gameObject);
    }
}
