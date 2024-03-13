using UnityEngine;

namespace Assets.Player.Scripts.FSM
{
    public interface IState
    {
        State state { get; } // 상태 ID
        bool canExecute { get; } // 상태가 실행 가능한지

        // Monobehaviour가 아닌 컨트롤러가 통제하기 때문에 On을 붙임
        // Unity 엔진에서는 OnEnter와 OnUpdate시마다 Animator 참조를 받음
        void OnEnter(Animator animator); // 진입
        State OnUpdate(Animator animator); // 업데이트
        void OnExit(Animator animator); // 탈출시 호출
    }
}
