using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStamina : MonoBehaviour
{
    Slider slider;
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;

    [SerializeField] private Image maskFlik;
    // Calcul stamina length
    private float _striLength;
    private RectTransform _rectTransform;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        _rectTransform = transform.GetComponent<RectTransform>();
        _striLength = _rectTransform.rect.width;
        _rectTransform.sizeDelta = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
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
    }
    private void OnDisable()
    {
        _staminaSO.FailUseStamina -= FailUseStamina;
        _staminaSO.OnValueChanged -= ChangeValue;
    }

    void ChangeValue(float value)
    {
        _rectTransform.sizeDelta = new Vector2 (_striLength * _staminaSO.MaxStamina, _rectTransform.sizeDelta.y);
        slider.value = value/_staminaSO.MaxStamina;
    }
}
