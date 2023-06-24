using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStamina : UISlider
{
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    [SerializeField] private Image maskFlik;
    
    private bool lerpUpdateCoroutine;

    private bool _isLerping = false;
    // Calcul stamina length
   
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent<RectTransform>();
        _striLength = _rectTransform.rect.width;
        _rectTransform.sizeDelta = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
        StartCoroutine(WaitToDoAnimation());
    }

    IEnumerator WaitToDoAnimation()
    {
        yield return new WaitForSeconds(5f);
        lerpUpdateCoroutine = true;
    }
 

    private void FailUseStamina()
    {
        StartCoroutine(Flick());
    }

    IEnumerator Flick()
    {
        maskFlik.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        maskFlik.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _staminaSO.FailUseStamina += FailUseStamina;
        _staminaSO.OnValueChanged += ChangeValue;
        _staminaSO.OnMaxValueChanged += MaxValueChanged;
    }

    private void MaxValueChanged(float obj)
    {
        if (lerpUpdateCoroutine)
        {
            OnMaxIncrease?.Invoke();
        }
    }

    private void OnDisable()
    {
        _staminaSO.FailUseStamina -= FailUseStamina;
        _staminaSO.OnValueChanged -= ChangeValue;
        _staminaSO.OnMaxValueChanged -= MaxValueChanged;
    }
    
    public override void UpdateStri()
    {
        if (lerpUpdateCoroutine)
        {
            StartCoroutine(UpdateCoroutine());
        }
        else
        {
            Vector2 start = _rectTransform.sizeDelta;
            Vector2 target = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
            _rectTransform.sizeDelta = Vector2.Lerp(start,target , 1);
        }
    }
    void ChangeValue(float value)
    {
        
       // _rectTransform.sizeDelta = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
       _slider.value = value/_staminaSO.MaxStamina;
    }
    public override IEnumerator UpdateCoroutine()
    {
        Vector2 start = _rectTransform.sizeDelta;
        Vector2 target = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
       
        _isLerping = true;
       

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _rectTransform.sizeDelta = Vector2.Lerp(start,target , t);
            yield return null;
        }

        _isLerping = false;
    }
}
