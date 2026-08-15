using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

public class PlayerCannonController : MonoBehaviour
{
    [SerializeField, NotNull] Cannon _cannon;

    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField, Range(0, 0.9f)] float _stickDistanceThreshold = 0.15f;

    [Header("Trace")]
    [SerializeField, NotNull] private GameObject _tracePrefab;

    private PlayerContext _playerContext;
    private VisualObjectPool _pool;

    private List<GameObject> _traces = new();

    private void Awake()
    {
        _pool = new(_tracePrefab);
    }

    private void Start()
    {
        _cannon.RotateTowards(transform.parent.forward);
    }

    private void Update()
    {
        if (_playerContext != null)
        {
            HandleAiming();
            HandleTraceVisualisation();
        }
    }

    private void OnDestroy()
    {
        foreach (var trace in _traces)
        {
            if (trace != null)
            {
                _pool.Pool.Release(trace);
            }
        }
        _traces.Clear();
    }

    public void Initialize(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void HandleAiming()
    {
        Quaternion cannonRot = _cannon.LocalRotation;
        Vector3 rotationOffset = CalculateRotationSpeed() * Time.deltaTime;
        if (rotationOffset != Vector3.zero && _playerContext.IsAiming)
        {
            Quaternion newLocalRotation = cannonRot * Quaternion.Euler(rotationOffset);
            _cannon.RotateLocally(newLocalRotation);
        }

        if (_playerContext.IsAttacking)
        {
            _cannon.TryShoot();
        }
    }

    private Vector3 CalculateRotationSpeed()
    {
        var cameraMovement = _playerContext.AimingStickPosition.Value;
        float magnitude = cameraMovement.magnitude;
        if (magnitude <= _stickDistanceThreshold)
        {
            return Vector3.zero;
        }

        float trueMagnitude = (magnitude - _stickDistanceThreshold) / (1 - _stickDistanceThreshold);
        return new Vector3(-cameraMovement.y, cameraMovement.x, 0).normalized * trueMagnitude * _rotationSpeed;
    }

    private void HandleTraceVisualisation()
    {
        List<Vector3> positions = new();
        if (_playerContext.IsAiming)
        {
            Vector3 cannonDir = _cannon.GlobalRotation * Vector3.forward;
            var steps = _cannon.GetTrace(cannonDir)[0];
            positions = SplitTrace(steps);
        }

        while (_traces.Count < positions.Count)
        {
            _traces.Add(_pool.Pool.Get());
        }
        while (_traces.Count > positions.Count)
        {
            var lastTrace = _traces[_traces.Count - 1];
            _pool.Pool.Release(lastTrace);
            _traces.RemoveAt(_traces.Count - 1);
        }

        for (int i = 0; i < positions.Count; i++)
        {
            _traces[i].transform.position = positions[i];
        }
    }

    private List<Vector3> SplitTrace(TraceNode[] steps)
    {
        List<Vector3> positions = new();
        positions.Add(steps[0].Start);

        foreach (var step in steps)
        {
            if (step.Hits == null)
            {
                continue;
            }
            if (step.Hits.Length > 0)
            {
                var firstHit = step.Hits[0];
                positions.Add(firstHit.point);
                break;
            }
            positions.Add(step.End);
        }
        return positions;
    }
}
