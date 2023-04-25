using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputHelper : MonoBehaviour
{
    [SerializeField] private Sprite _iconObject;
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float maxDistanceToShow = 45;
    [Tooltip("If is true, the UI Element is not connected with the manipulation of this component")]
    [SerializeField] private bool _UIElementIsIndependant;
    private UIInputHelper _uiInputHelper;
    // Start is called before the first frame update
    void Start()
    {
        _uiInputHelper = UIInputHelper.CreateInputHelper(_prefab, transform, _iconObject , maxDistanceToShow,_inputActionReference);
        
    }

    private void OnDisable()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null)
            _uiInputHelper.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null)
            _uiInputHelper.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if(!_UIElementIsIndependant && _uiInputHelper != null)
            Destroy(_uiInputHelper.gameObject);
    }
}
