using UnityEngine;

namespace Character.Player
{
    public class AimingController : IUpdateControls
    {
        private readonly PlayerContext _context;

        private ReactiveField<Vector2> _stickPos;

        private ReactiveListener<Vector2> _cameraMovement;
        private ReactiveListener<bool> _isAiming;

        private Vector2 _truePos;

        private float _maxRange = 400f;

        public AimingController(PlayerContext context)
        {
            _context = context;
            _stickPos = _context.AimingStickPosition;
            _cameraMovement = _context.CameraMovement;
            _isAiming = _context.IsAiming;
            _truePos = Vector2.zero;
        }

        public void UpdateControls()
        {
            if (_isAiming.Value == false)
            {
                _truePos = Vector2.zero;
            }
            else
            {
                _truePos += _cameraMovement.Value;
                _truePos = Vector2.ClampMagnitude(_truePos, _maxRange);
            }
            _stickPos.Value = _truePos / _maxRange;
        }
    }
}