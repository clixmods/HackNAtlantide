using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHealthPlayer : UISlider
{
    [SerializeField] private ScriptableValueFloat healthValue;
    private bool lerpUpdateCoroutine ;
    [SerializeField] private LagFill lagfill;
   
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent<RectTransform>();
        _striLength = _rectTransform.rect.width;
        StartCoroutine(WaitToDoAnimation());
    }

    private void Start()
    {
        healthValue.OnValueChanged += OnValueChanged;
        healthValue.OnMaxValueChanged += MaxValueChanged;
        _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.Value01;
    }

    IEnumerator WaitToDoAnimation()
    {
        yield return new WaitForSeconds(5f);
        lerpUpdateCoroutine = true;
    }
    private void MaxValueChanged(float currentMaxValue)
    {
        // _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        // _slider.value = healthValue.Value01;
        OnMaxIncrease?.Invoke();
    }
    
    public override IEnumerator UpdateCoroutine()
    {
        Vector2 start = _rectTransform.sizeDelta;
        Vector2 target = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _rectTransform.sizeDelta = Vector2.Lerp(start,target , t);
            lagfill.SetLagFillToTargetDirectly();
            yield return null;
        }
        
        _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.Value01; // Need to update the slider value to keep correct information on screen
        lagfill.SetLagFillToTargetDirectly();
    }

  
    private void OnValueChanged(float currentValue)
    {
        _rectTransform.sizeDelta = new Vector2 (_striLength * healthValue.MaxValue, _rectTransform.sizeDelta.y);
        _slider.value = healthValue.Value01;
    }

    private void OnDestroy()
    {
        healthValue.OnValueChanged -= OnValueChanged;
        healthValue.OnMaxValueChanged -= MaxValueChanged;
    }
}
