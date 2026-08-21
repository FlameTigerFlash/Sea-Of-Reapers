using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerContext _playerContext;

        public void Initialize(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public void OnMove(InputAction.CallbackContext callbackContext)
        {
            if (_playerContext == null) return;

            Vector2 dir = callbackContext.ReadValue<Vector2>();
            _playerContext.RequestedDirection.Value = dir;
        }

        public void OnAttack(InputAction.CallbackContext callbackContext)
        {
            if (_playerContext == null) return;

            if (callbackContext.canceled)
            {
                _playerContext.IsAttacking.Value = false;
            }
            else
            {
                _playerContext.IsAttacking.Value = true;
            }
        }

        public void OnAuxAction(InputAction.CallbackContext callbackContext)
        {
            if (_playerContext == null) return;

            if (callbackContext.canceled)
            {
                _playerContext.IsAiming.Value = false;
            }
            else
            {
                _playerContext.IsAiming.Value = true;
            }
        }

        public void OnLook(InputAction.CallbackContext callbackContext)
        {
            if (_playerContext == null) return;

            Vector2 offset = callbackContext.ReadValue<Vector2>();
            _playerContext.CameraMovement.Value = offset;
        }

        public void OnObserve(InputAction.CallbackContext callbackContext)
        {
            if (_playerContext == null) return;

            _playerContext.IsObserving.Value = !(callbackContext.canceled);
        }
    }
}