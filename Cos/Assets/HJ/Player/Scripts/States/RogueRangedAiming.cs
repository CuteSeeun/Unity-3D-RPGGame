using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class RogueRangedAiming : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<AniOnEvent_Rogue>().USE_CROSSBOW();
        }
    }
}
