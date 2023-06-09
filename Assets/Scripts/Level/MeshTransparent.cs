using System;
using System.Collections.Generic;
using UnityEngine;
public class MeshTransparent : MonoBehaviour
{
    Material[] _baseMaterials;
    [SerializeField] private Material materialTransparent;
    private List<MeshTransparent> _meshTransparentWatchers = new List<MeshTransparent>();
    [SerializeField] private bool createMeshTransparentInChild = false; 
    private bool _isHide;
    public bool IsHide
    {
        get
        {
            return _isHide;
        }
        set
        {
            foreach (var t in _meshTransparentWatchers)
            {
                t.IsHide = value;
            }

            _isHide = value;
        }
    }
    private MeshRenderer _meshRenderer;
    private bool _isInit;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private const float TransitionSpeedMultiplier = 3;

    private void Awake()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();

        if (createMeshTransparentInChild)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<MeshRenderer>(out var meshrenderer))
                {
                    var component = child.gameObject.AddComponent<MeshTransparent>();
                    component.materialTransparent = materialTransparent;
                    _meshTransparentWatchers.Add(component);
                }
                
            }
        }
        
    }

    private void Start()
    {
        Init(materialTransparent);
    }

    public void Init(Material materialTransparentToApply) 
    {
        _baseMaterials = _meshRenderer.sharedMaterials;             
        _isInit = true;
        transform.gameObject.layer = 9;
        materialTransparent = new Material(materialTransparent);
    }
    private void Update()
    {
        
        if(!_isInit) return;
        Color _color = materialTransparent.GetColor(BaseColor); 
        if(IsHide)
        {
            if(_color.a > 0.23f)
            {
                _color.a -= Time.deltaTime*TransitionSpeedMultiplier;

            }
            else
            {
                _color.a = 0.23f;
            }
            materialTransparent.SetColor(BaseColor, _color);


            Material[] materialsToChange = _meshRenderer.sharedMaterials;
            for(int i = 0 ; i < materialsToChange.Length ; i++)
                materialsToChange[i] = materialTransparent;
                
            _meshRenderer.sharedMaterials = materialsToChange;
        }
        else
        {
            if(_color.a < 1)
            {
                _color.a += Time.deltaTime*TransitionSpeedMultiplier*2;
            }
            else
            {
                _meshRenderer.sharedMaterials = _baseMaterials;
                _color.a = 1;
            }
            materialTransparent.SetColor(BaseColor, _color);
           
        }
    }
    private void LateUpdate()
    {
        IsHide = false;
    }
}