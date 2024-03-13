using UnityEngine;

namespace Assets.Player.Scripts.FSM
{
    public class Move : StateBase
    {
        public override State state => State.Move;

        public override State OnUpdate(Animator animator)
        {
            State next = base.OnUpdate(animator);

            return next;
        }
    }
}
