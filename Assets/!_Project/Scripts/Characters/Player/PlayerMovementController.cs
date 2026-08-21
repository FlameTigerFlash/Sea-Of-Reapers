using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Character.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Engine")]
        [SerializeField, NotNull] private Engine _engine;
        [SerializeField] private Transform _engineRotationPoint;

        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField, Range(0, 90f)] private float _maxRotation = 45f;
        [SerializeField, Min(0f)] private float _maxForwardThrust = 30000f;
        [SerializeField, Min(0f)] private float _maxBackwardThrust = 20000f;

        private float _desiredRotation = 0f;

        private PlayerContext _playerContext;

        private void OnValidate()
        {

        }

        private void Update()
        {
            if (_playerContext == null) return;

            var requestedDirection = _playerContext.RequestedDirection;

            _desiredRotation = requestedDirection.Value.x * _maxRotation;

            Vector3 curRot = _engineRotationPoint.localEulerAngles;
            curRot.y = Mathf.LerpAngle(curRot.y, -_desiredRotation, _rotationSpeed * Time.deltaTime);
            _engineRotationPoint.localEulerAngles = curRot;

            var currentDir = new Vector2(Mathf.Sin(-Mathf.Deg2Rad * curRot.y), Mathf.Cos(Mathf.Deg2Rad * curRot.y));
            float thrustCoef = _maxForwardThrust * Vector2.Dot(requestedDirection, currentDir);
            if (thrustCoef >= 0)
            {
                _engine.Thrust = _maxForwardThrust * Vector2.Dot(requestedDirection, currentDir);
            }
            else
            {
                _engine.Thrust = _maxBackwardThrust * Vector2.Dot(requestedDirection, currentDir);
            }
        }

        public void Initialize(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }
    }
}