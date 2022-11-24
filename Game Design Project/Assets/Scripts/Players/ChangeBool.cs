using UnityEngine;

public class ChangeBool : StateMachineBehaviour
{
    public string boolName;
    public bool status;
    public bool resetOnExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (resetOnExit)
            animator.SetBool(boolName, !status);
    }
}
