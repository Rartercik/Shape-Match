using UnityEngine;
using Game.Figures;

namespace Game.States
{
    public class Process : GameState
    {
        private readonly GameObject _processObjects;
        private readonly FiguresGenerator _figuresGenerator;
        private readonly FiguresBuffer _figuresBuffer;

        public Process(GameStatesHandler statesHandler, GameObject processObjects, FiguresGenerator figuresGenerator, FiguresBuffer figuresBuffer) : base(statesHandler)
        {
            _processObjects = processObjects;
            _figuresGenerator = figuresGenerator;
            _figuresBuffer = figuresBuffer;
        }

        public override void Enter()
        {
            _processObjects.SetActive(true);
            _figuresGenerator.Generate();
        }

        public override void Exit()
        {
            _figuresGenerator.ResetState();
            _figuresBuffer.ResetState();
            _processObjects.SetActive(false);
        }

        public override void ReportVictory()
        {
            StatesHandler.SwitchState<Victory>();
        }

        public override void ReportDefeat()
        {
            StatesHandler.SwitchState<Defeat>();
        }
    }
}