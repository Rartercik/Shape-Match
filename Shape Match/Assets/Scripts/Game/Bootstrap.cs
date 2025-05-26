using UnityEngine;
using Game.States;
using Game.Figures;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private GameStatesHandler _gameStatesHandler;
        [SerializeField] private FiguresGenerator _figuresGenerator;
        [SerializeField] private FiguresBuffer _figuresBuffer;
        [SerializeField] private uint _groupsSize;

        private void Awake()
        {
            _input.Initialize();
            _gameStatesHandler.Initialize(_figuresGenerator, _figuresBuffer);
            _figuresGenerator.Initialize(_figuresBuffer, _groupsSize);
            _figuresBuffer.Initialize(_groupsSize);
        }
    }
}