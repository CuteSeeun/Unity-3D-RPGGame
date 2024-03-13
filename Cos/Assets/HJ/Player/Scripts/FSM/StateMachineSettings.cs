using System.Collections.Generic;

namespace Assets.Player.Scripts.FSM
{
    public static class StateMachineSettings
    {
        private static Dictionary<State, IState> s_PlayerStates = new Dictionary<State, IState>()
        {
            {State.Move, new Move() },
            {State.Dodge, new Attack1() },
        };

            public static IDictionary<State, IState> GetPlayerStates(CharacterController playerController)
        {
            return s_PlayerStates;
        }
    }
}
