using UnityEngine;
using UnityEngine.InputSystem;
using Game.Figures;

namespace Game
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private FiguresDestroyer _figuresMatcher;

        private InputSystemActions _playerInput;
        private InputAction _interact;

        public void Initialize()
        {
            _playerInput = new InputSystemActions();
            _interact = _playerInput.Player.Interact;
            _interact.performed += TryDeleteFigure;
        }

        private void OnEnable()
        {
            _interact.Enable();
        }

        private void OnDisable()
        {
            _interact.Disable();
        }

        private void TryDeleteFigure(InputAction.CallbackContext context)
        {
            var position = context.ReadValue<Vector2>();
            position = _camera.ScreenToWorldPoint(position);
            _figuresMatcher.TryDeleteFigure(position);
        }
    }
}