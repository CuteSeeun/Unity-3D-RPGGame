using UnityEngine;

namespace Assets.Player.Scripts.FSM
{
    // 다른 State들의 베이스가 되는 추상 클래스
    public abstract class StateBase : IState
    {
        // override 하여 각자에게 맞는 State를 반환
        public abstract State state { get; }

        // 기본형으로 사용하되 필요한 경우 override
        public virtual bool canExecute => true;

        public virtual void OnEnter(Animator animator)
        {
        }

        public virtual void OnExit(Animator animator)
        {
        }

        // 기본적으로 자신 State를 반환하여 Machine이 계속 자신을 호출할 수 있도록 함.
        public virtual State OnUpdate(Animator animator)
        {
            return state;
        }
    }
}
