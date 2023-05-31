using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : StateMachineBehaviour
{
    Character character;
    [SerializeField] float maxSpeed;
    int idWalkSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null)
        {
            character = animator.GetComponent<Character>();
            idWalkSpeed = Animator.StringToHash("SpeedWalk");
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(idWalkSpeed, Mathf.Lerp(0.05f, character.CurrentSpeed / maxSpeed, character.CurrentSpeed));
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(idWalkSpeed, 1);
    }
}
