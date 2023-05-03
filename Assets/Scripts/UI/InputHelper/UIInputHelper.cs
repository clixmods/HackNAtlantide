using System;
using System.Linq;
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
    [SerializeField] protected Image _imageInput;
    protected InputActionReference _inputActionReference;
    [SerializeField] protected InputActionIcons inputActionIcons;
    [SerializeField] private float distanceToShow = 10;
    protected static Canvas _canvas;
    
    public static UIInputHelper CreateInputHelper(GameObject prefab, Transform transformTarget, Sprite image, float maxDistanceToShow,
        InputActionReference input = default)
    {
        if (transformTarget == null) return null;

        if (_canvas == null)
        {
            _canvas = FindObjectOfType<Canvas>();
        }
        var inputHelperObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, _canvas.transform);
        UIInputHelper component = inputHelperObject.GetComponentInChildren<UIInputHelper>().Init(prefab, transformTarget,  image,  maxDistanceToShow, input);
        return component;
    }

    public virtual UIInputHelper Init(GameObject prefab, Transform transformTarget, Sprite image, float maxDistanceToShow,
        InputActionReference input = default)
    {
        // Setup
        this._inputActionReference = input;
        if (image != null)
        {
            this._imageIcon.sprite = image;
        }
        else
        {
            this._imageIcon.gameObject.SetActive(false);
        }
        this._targetTransform = transformTarget;
        this.distanceToShow = maxDistanceToShow;
        return this;
    }
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerControls ??= new PlayerControls();
        _initialScale = ((RectTransform) transform).sizeDelta;
        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
        // Apply default binding group
        _bindingGroup = _playerControls.controlSchemes[0].bindingGroup;
        
        
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
        UpdateOpacity();
    }
    protected virtual void UpdateInputIcon()
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
        
        var distance = Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position, _targetTransform.position);
        var color = _imageInput.color;
        color.a = 1-Mathf.Clamp((distance / distanceToShow),0,1);
        _imageInput.color = color;
    }
    private void UpdateScale()
    {
        
        var distance = Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position, _targetTransform.position);
        var calcul = distance / distanceToShow ;
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
