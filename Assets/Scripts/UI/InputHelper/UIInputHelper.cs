using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

[AddComponentMenu("#Survival Game/UI/UI Input Helper")]
public class UIInputHelper : MonoBehaviour 
{
    private static PlayerControls _playerControls ;
    private Vector3 _offset;
    private Transform _targetTransform;
    private Vector2 _initialScale;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Image _imageInput;
    private InputActionReference _inputActionReference;
    [SerializeField] InputActionIcons inputActionIcons;
    private static Canvas _canvas;
    public static UIInputHelper CreateInputHelper(GameObject prefab, Transform transformTarget,Sprite image, InputActionReference input = default)
    {
        UIInputHelper component = null;
        if (transformTarget == null) return null;

        if (_canvas == null)
        {
            _canvas = FindObjectOfType<Canvas>();
        }
        var inputHelperObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, _canvas.transform);
        // Setup
        component = inputHelperObject.GetComponentInChildren<UIInputHelper>();
        component._inputActionReference = input;
        if (image != null)
        {
            component._imageIcon.sprite = image;
        }
        else
        {
            component._imageIcon.gameObject.SetActive(false);
        }
        component._targetTransform = transformTarget;
        return component;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerControls ??= new PlayerControls();
        _initialScale = ((RectTransform) transform).sizeDelta;
        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
    }
        private string _bindingGroup;
    public string bindingGroup => _bindingGroup;
     private void OnButtonPressed(InputControl button)
    {
        var list = _playerControls.controlSchemes.ToArray();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].SupportsDevice(button.device))
            {
                _bindingGroup = list[i].bindingGroup;
            }
        }
    }
    private void Start()
    {
      
        _initialScale = ((RectTransform) transform).sizeDelta;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateInputIcon();
        UpdatePosition();
        UpdateScale();
        //UpdateOpacity();
    }
    private void UpdateInputIcon()
    {
        if (_inputActionReference != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    _imageInput.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path];
                    break;
                }
            }
        }
    }
    private void UpdateOpacity()
    {
        var distance = Vector3.Distance(CameraUtility.Camera.transform.position, _targetTransform.position);
        var color = _imageInput.color;
        color.a = 1-Mathf.Clamp((distance / 10f),0,1);
        _imageInput.color = color;
    }
    private void UpdateScale()
    {
        var distance = Vector3.Distance(CameraUtility.Camera.transform.position, _targetTransform.position);
        var calcul = distance / 10f ;
        var clampT = Mathf.Clamp(calcul, 0, 1);
        ((RectTransform) transform).sizeDelta = Vector2.Lerp(_initialScale , _initialScale * 0.5f, clampT);
    }

    void UpdatePosition()
    {
        // Si l'object a follow est detruit, on le delete
        if(_targetTransform == null)
        {
            transform.position = new Vector3(-10000,0,-100);
            return;
        }
        Vector3 position = _targetTransform.position.GetPositionInWorldToScreenPoint();
        if(_targetTransform.position.IsOutOfCameraVision() )
        {
            transform.position = new Vector3(-10000,0,-100);
        }
        else
        {
            transform.position = position + _offset;
        }
    }
}
