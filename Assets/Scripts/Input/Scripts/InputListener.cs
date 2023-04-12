using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    [SerializeField] InputVectorScriptableObject _move;
    // Start is called before the first frame update
    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
        _move.OnValueChanged += MoveInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
        _move.OnValueChanged -= MoveInput;
    }
    void InteractInput(bool value)
    {
        Debug.Log(value);
    }
    void MoveInput(Vector2 value)
    {
        Debug.Log(value);
    }
}
