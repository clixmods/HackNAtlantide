using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UISlider : MonoBehaviour
{
    protected Slider _slider;
    protected RectTransform _rectTransform;
    protected float _striLength;
    public UnityEvent OnMaxIncrease;
    public void UpdateStri()
    {
        
        StartCoroutine(UpdateCoroutine());
    }
    public abstract IEnumerator UpdateCoroutine();
}
