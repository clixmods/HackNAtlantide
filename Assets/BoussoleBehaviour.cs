using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoussoleBehaviour : MonoBehaviour
{
    [SerializeField] private InteractDetection _interactDetection;
    bool _interactObjectIsClose;
    bool _isUsingBoussole;
    [SerializeField] GameObject _interfaceBoussole;
    [SerializeField] Transform _aiguille;
    [SerializeField] InputButtonScriptableObject _inputBoussole;
    private float timeToChangeAiguilleDirection = 1f;
    float _timeReset;
    float speedBoussoleRotate;
    float speedTemp = 1;
    private void OnEnable()
    {
        _inputBoussole.OnValueChanged += Input;
    }
    private void OnDisable()
    {
        _inputBoussole.OnValueChanged -= Input;
    }
    private void Start()
    {
        _timeReset = timeToChangeAiguilleDirection;
    }
    private void Update()
    {
        if(_isUsingBoussole)
        {
            List<IInteractable> interactables = _interactDetection.Interactable;
            if (interactables.Count > 0)
            {
                BoussoleControl(true);
            }
            else
            {
                BoussoleControl(false);
            }
            
        }
    }
    void BoussoleControl(bool _isDetract)
    {
        timeToChangeAiguilleDirection -= Time.deltaTime;
        if(timeToChangeAiguilleDirection < 0)
        {
            timeToChangeAiguilleDirection = _timeReset;
            speedBoussoleRotate = (Random.value-0.5f)*2;
        }
        speedTemp = Mathf.Lerp(speedTemp, speedBoussoleRotate, 1 - timeToChangeAiguilleDirection / _timeReset);

        if (_isDetract)
        {
            _aiguille.transform.Rotate(Vector3.forward, 1500*speedTemp * Time.deltaTime);
        }
        else
        {
            _aiguille.transform.Rotate(Vector3.forward, -100 *speedTemp* Time.deltaTime);
        }
    }
    void Input(bool perfomed)
    {
        _isUsingBoussole = perfomed;
        _interfaceBoussole.SetActive(perfomed);
    }
}
