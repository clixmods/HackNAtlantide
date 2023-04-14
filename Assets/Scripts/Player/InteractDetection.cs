using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractDetection : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    bool _canInteract = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
    }
    void InteractInput(bool value)
    {
        //if(value)

    }
    private void OnBecameVisible()
    {
        _canInteract = true;
    }
    private void OnBecameInvisible()
    {
        _canInteract = false;
    }
}
