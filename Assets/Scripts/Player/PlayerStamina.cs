using UnityEngine;
public class PlayerStamina : MonoBehaviour
{
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    [SerializeField] ParticleSystem _useStaminaFX;
    [SerializeField] ParticleSystem _failUseStaminaFX;

    public PlayerStaminaScriptableObject StaminaData => _staminaSO;

    private void OnEnable()
    {
        _staminaSO.OnUseStamina += RemoveStamina;
        _staminaSO.FailUseStamina += FailStamina;
    }
    private void OnDisable()
    {
        _staminaSO.OnUseStamina -= RemoveStamina;
        _staminaSO.FailUseStamina -= FailStamina;
    }
    private void Start()
    {
        _staminaSO.ResetStamina();
        _staminaSO.CanRechargeStamina = true;
    }
    private void Update()
    {
        if(!_staminaSO.IsMaxStamina() && _staminaSO.CanRechargeStamina)
        {
            _staminaSO.Value += Time.deltaTime * _staminaSO.SpeedToRecharge;
        }
    }
    public void RemoveStamina(float value)
    {
        
        _staminaSO.Value -= value;
        if(_staminaSO.Value<0)
        {
            _staminaSO.Value = 0;
            _staminaSO.OnStaminaIsEmpty?.Invoke();
        }

        //feedBack
        _useStaminaFX.Play();
    }
    
    void FailStamina()
    {
        _failUseStaminaFX.Play();
    }
}
