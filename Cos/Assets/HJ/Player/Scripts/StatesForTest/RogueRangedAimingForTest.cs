using UnityEngine;
using CharacterControllerForTest = HJ.CharacterControllerForTest;

namespace HJ
{
    public class RogueRangedAimingForTest : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<AniOnEvent_Rogue>().USE_CROSSBOW();
        }
    }
}
