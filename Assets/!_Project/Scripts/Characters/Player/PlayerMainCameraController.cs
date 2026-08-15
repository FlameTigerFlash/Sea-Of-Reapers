using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class PlayerMainCameraController : MonoBehaviour
{
    [SerializeField, NotNull] private CinemachineCamera _camera;
    [SerializeField, NotNull] private CinemachineOrbitalFollow _orbitalFollow;

    [SerializeField, NotNull] private float _verticalRotationSpeed = 0.4f;
    [SerializeField, NotNull] private float _horizontalRotationSpeed = 0.4f;

    private PlayerContext _context;

    private bool _isObserving = false;

    private void OnValidate()
    {
        _camera = _camera != null ? _camera : GetComponentInChildren<CinemachineCamera>();
        _orbitalFollow = _orbitalFollow != null ? _orbitalFollow : GetComponentInChildren<CinemachineOrbitalFollow>();
    }

    private void Update()
    {
        if (_context == null) return;

        bool newObservationState = _context.IsObserving.Value;

        if (newObservationState)
        {
            HandleWorldView();
        }
        else
        {
            HandleLocalView();
        }
        _isObserving = newObservationState;
    }

    public void Initialize(PlayerContext context)
    {
        _context = context;
    }

    private void HandleWorldView()
    {
        if (!_isObserving)
        {
            CancelRecenteringAll();

            _orbitalFollow.TrackerSettings.BindingMode = BindingMode.WorldSpace;
            _orbitalFollow.ForceCameraPosition(_camera.transform.position, _camera.transform.rotation);
        }

        if (!_context.IsAiming)
        {
            Vector2 cameraMovement = _context.CameraMovement.Value;
            cameraMovement.x *= _horizontalRotationSpeed;
            cameraMovement.y *= _verticalRotationSpeed;

            _orbitalFollow.HorizontalAxis.Value += cameraMovement.x;
            _orbitalFollow.VerticalAxis.Value += cameraMovement.y;
        }
    }

    private void HandleLocalView()
    {

        if (_isObserving)
        {
            _orbitalFollow.TrackerSettings.BindingMode = BindingMode.LazyFollow;
            _orbitalFollow.ForceCameraPosition(_camera.transform.position, _camera.transform.rotation);

            RecenterAll();
        }
    }

    private void RecenterAll()
    {
        _orbitalFollow.VerticalAxis.Recentering.Enabled = true;
        _orbitalFollow.VerticalAxis.TriggerRecentering();

        _orbitalFollow.HorizontalAxis.Recentering.Enabled = true;
        _orbitalFollow.HorizontalAxis.TriggerRecentering();
    }

    private void CancelRecenteringAll()
    {
        _orbitalFollow.VerticalAxis.Recentering.Enabled = false;
        _orbitalFollow.VerticalAxis.CancelRecentering();

        _orbitalFollow.HorizontalAxis.Recentering.Enabled = false;
        _orbitalFollow.HorizontalAxis.CancelRecentering();
    }
}
