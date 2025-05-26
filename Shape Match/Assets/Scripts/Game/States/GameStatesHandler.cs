using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Figures;

namespace Game.States
{
    public class GameStatesHandler : MonoBehaviour
    {
        [SerializeField] private Canvas _preparationCanvas;
        [SerializeField] private GameObject _processObjects;
        [SerializeField] private Canvas _victoryCanvas;
        [SerializeField] private Canvas _defeatCanvas;

        private GameState[] _states;

        public GameState State { get; private set; }

        public void Initialize(FiguresGenerator figuresGenerator, FiguresBuffer figuresBuffer)
        {
            _states = CreateAllStates(figuresGenerator, figuresBuffer, out var defaultState);

            State = defaultState;
        }

        public void ReportBeginProcess()
        {
            State.ReportBeginProcess();
        }

        public void ReportVictory()
        {
            State.ReportVictory();
        }

        public void ReportDefeat()
        {
            State.ReportDefeat();
        }

        public void ReportPreparationRequest()
        {
            State.ReportPreparationRequest();
        }

        public void SwitchState<T>() where T : GameState
        {
            State.Exit();
            State = _states.First(s => s is T);
            State.Enter();
        }

        private GameState[] CreateAllStates(FiguresGenerator figuresGenerator, FiguresBuffer figuresBuffer, out GameState defaultState)
        {
            var states = new List<GameState>();
            defaultState = new Preparation(this, _preparationCanvas);

            states.Add(defaultState);
            states.Add(new Process(this, _processObjects, figuresGenerator, figuresBuffer));
            states.Add(new Victory(this, _victoryCanvas));
            states.Add(new Defeat(this,  _defeatCanvas));

            return states.ToArray();
        }
    }
}
