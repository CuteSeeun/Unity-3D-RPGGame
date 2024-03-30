using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    public class UseItem : UniversialStateScript
    {
        [Header ("Use Item")]
        [Range(0, 2f)]
        [SerializeField] float _delayTime;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            //_playerController.Invoke("UsePotion", _delayTime * _stateLength);
        }
    }
}
