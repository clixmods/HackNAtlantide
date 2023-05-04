using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LagFill : MonoBehaviour
{
   private Slider _slider;
   [SerializeField] private RectTransform fillLagRectTransform;
   [Header("Settings")]
   [SerializeField] private float delayToStartLag = 3;
   [SerializeField]private float interpolationDuration = 5f; 
   private float t;
   

   private void Awake()
   {
      _slider = GetComponent<Slider>();
      _slider.onValueChanged.AddListener(OnValueChanged);
      this.enabled = false;
      
   }

   void OnValueChanged(float value)
   {
      this.enabled = true;
      delayToStartLag = 3;
      t = 0;
   }

   private void Update()
   {
      if (delayToStartLag > 0)
      {
         delayToStartLag -= Time.deltaTime;
      }
      else
      {
         t += Time.deltaTime;
         float interpo = t / interpolationDuration; 
         fillLagRectTransform.anchorMax = Vector2.Lerp(fillLagRectTransform.anchorMax, _slider.fillRect.anchorMax, interpo );
      }
   }
}
