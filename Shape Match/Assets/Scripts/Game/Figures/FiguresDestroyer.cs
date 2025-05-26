using UnityEngine;
using Game.States;

namespace Game.Figures
{
    public class FiguresDestroyer : MonoBehaviour
    {
        [SerializeField] private LayerMask _selectable;
        [SerializeField] private GameStatesHandler _statesHandler;
        [SerializeField] private FiguresGenerator _generator;
        [SerializeField] private FiguresBuffer _buffer;

        private int _remainingAmount;

        private void OnEnable()
        {
            _generator.OnGenerated += SetRemainingAmount;
        }

        private void OnDisable()
        {
            _generator.OnGenerated -= SetRemainingAmount;
        }

        public void TryDeleteFigure(Vector2 position)
        {
            var collider = Physics2D.OverlapPoint(position, _selectable);
            if (collider != null && collider.TryGetComponent<Figure>(out var figure))
            {
                _buffer.AddFigure(figure.Content);
                Destroy(figure.gameObject);
                _remainingAmount--;
                if (_remainingAmount == 0)
                {
                    _statesHandler.ReportVictory();
                }
            }
        }

        private void SetRemainingAmount(int remainingAmount)
        {
            _remainingAmount = remainingAmount;
        }
    }
}