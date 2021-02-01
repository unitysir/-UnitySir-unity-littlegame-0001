using UnityEngine;

public class InStandResetTrigger : StateMachineBehaviour
{
    public string[] clearEnter;

    public string[] clearExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var enter in clearEnter)
        {
            animator.ResetTrigger(enter);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var exit in clearExit)
        {
            animator.ResetTrigger(exit);
        }
    }
}