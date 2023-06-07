using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Value/float")]
public class ScriptableValueFloat : ScriptableValue<float>
{
    [SerializeField] private float _maxValue;
    public Action<float> OnMaxValueChanged;
    [Header("Default")]
    [SerializeField] protected float defaultValue = 0;
    [SerializeField] protected float defaultMaxValue = 0;
    #region Properties

    public override float Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }
    public float DefaultValue => defaultValue;
    public float DefaultMaxValue => defaultMaxValue;
    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            OnMaxValueChanged?.Invoke(value);
        }
    }

    public float Value01 => Value / MaxValue;

    #endregion
    #region Methods
    public void SetValueToMaxValue()
    {
        Value = MaxValue;
    }

    public void IncrementMaxValue(float incrementValue)
    {
        MaxValue += incrementValue;
    }
    public void DecrementMaxValue(float decrementValue)
    {
        MaxValue -= decrementValue;
    }
    public void IncrementValue(float incrementValue)
    {
        Value += incrementValue;
    }
    public void DecrementValue(float decrementValue)
    {
        Value -= decrementValue;
    }
    #endregion
}
