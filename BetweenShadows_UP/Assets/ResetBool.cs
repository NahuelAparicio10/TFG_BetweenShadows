using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public bool isInteracting;
    private const string _isInteractingName = "isInteracting";
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingName, isInteracting);
    }

    
}
