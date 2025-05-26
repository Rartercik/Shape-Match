using UnityEngine;

namespace Game.States
{
    public class Preparation : GameState
    {
        private readonly Canvas _canvas;

        public Preparation(GameStatesHandler statesHandler, Canvas canvas) : base(statesHandler)
        {
            _canvas = canvas;
        }

        public override void Enter()
        {
            _canvas.enabled = true;
        }

        public override void Exit()
        {
            _canvas.enabled = false;
        }

        public override void ReportBeginProcess()
        {
            StatesHandler.SwitchState<Process>();
        }
    }
}