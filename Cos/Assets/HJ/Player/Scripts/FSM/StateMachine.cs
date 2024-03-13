using System.Collections.Generic;
using UnityEngine;
using CharacterController = Assets.Player.Scripts.CharacterController;

namespace Assets.Player.Scripts.FSM
{
    internal class StateMachine
    {
        public StateMachine(CharacterController owner, IDictionary<State, IState> states)
        {
            this.owner = owner;
            this.states = new Dictionary<State, IState>(states);
            _animator = owner.GetComponent<Animator>();
        }

        // Controller란?
        // Data에 접근해서 Data의 갱신을 요청하고 Data가 갱신되면 갱신된 데이터를 바탕으로 유저에게 전달
        // Machine을 가지는 주인에 대한 참조
        public CharacterController owner;

        public State current; // 현재 상태
        private Dictionary<State, IState> states; // State와 사용할 인터페이스

        private Animator _animator;

        // Monobehaviour가 아닌 컨트롤러가 통제하기 때문에 On을 붙임
        public void OnUpdate()
        {
            // 매 프레임마다 OnUpdate가 반환하는 다음 상태로 전환
            ChangeState(
                states[current].OnUpdate(_animator) // current State에 해당하는 IState를 불러와 실행, 다음 상태를 반환
                );
        }

        // 다음 State로 전환
        public bool ChangeState(State newState)
        {
            // 현재 상태와 다음 상태가 동일하다면 바꿀 필요가 없음.
            if (newState == current)
                return false;
            // newState에 해당하는 인터페이스가 실행 가능하지 않다면 상태를 바꾸지 않음
            if (! states[newState].canExecute)
                return false;

            // 기존 상태에서 탈출
            states[current].OnExit(_animator);
            // 상태의 번호를 애니메이터에 전달
            _animator.SetInteger("state", (int)newState);
            // 새 상태에 진입
            states[newState].OnEnter(_animator);
            // current를 갱신
            current = newState;
            return true;
        }
    }
}
