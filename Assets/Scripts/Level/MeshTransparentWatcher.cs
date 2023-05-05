using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshTransparentWatcher : MonoBehaviour
{
    Material[] _baseMaterials;
    private Material _materialTransparent;
    public bool IsHide;
    private MeshRenderer _meshRenderer;
    private bool _isInit;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private const float TransitionSpeedMultiplier = 3;

    private void Awake()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    public void Init(Material materialTransparentToApply) 
    {
        _baseMaterials = _meshRenderer.sharedMaterials;             
        _isInit = true;
        transform.gameObject.layer = 9;
        _materialTransparent = new Material(materialTransparentToApply);
    }
    private void FixedUpdate()
    {
        if(!_isInit) return;
        Color _color = _materialTransparent.GetColor(BaseColor); 
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
            _materialTransparent.SetColor(BaseColor, _color);


            Material[] materialsToChange = _meshRenderer.sharedMaterials;
            for(int i = 0 ; i < materialsToChange.Length ; i++)
                materialsToChange[i] = _materialTransparent;
                
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
            _materialTransparent.SetColor(BaseColor, _color);
           
        }
    }
    private void LateUpdate()
    {
        IsHide = false;
    }
}