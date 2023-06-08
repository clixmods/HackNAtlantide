using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{
    [SerializeField] private InputButtonScriptableObject _inputAttack;
    private MeleeBaseState _meleeBaseState;
    private StateMachine meleeStateMachine;

    private void OnEnable()
    {
        _inputAttack.OnValueChanged += Attack;
    }
    private void OnDisable()
    {
        _inputAttack.OnValueChanged -= Attack;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    private void Attack(bool value)
    {
        if (value)
        {
            if (meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                meleeStateMachine.SetNextState(new GroundEntryState());
            }
        }
    }
}