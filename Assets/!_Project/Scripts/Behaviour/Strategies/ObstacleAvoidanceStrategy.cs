using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Character.Enemy;

public class ObstacleAvoidanceStrategy : IContextStrategy
{
    #region fields
    public float CalculationInterval
    {
        get
        {
            return _calculationInterval;
        }
        set
        {
            _calculationInterval = Mathf.Max(0, value);
        }
    }

    private ShipContext _enemyContext;

    private ReactiveListener<Vector3> _destinationPosListener;

    private ReactiveListener<GameObject> _targetGameObject;
    private GameObject _gameObject;
    private Rigidbody _rb;
    private Radar _radar;

    private ReactiveField<Vector3> _targetPositionField;

    private float _distanceCoef = 1f;
    private float _difficultyCoef = 0f;
    private float _calculationInterval = 1f;
    private float _lastCalculationTime = 0f;
    #endregion

    public void Initialize(ShipContext context)
    {
        _enemyContext = context;

        _destinationPosListener = context.MovementDestination;

        _gameObject = _enemyContext.SelfObject;
        _targetGameObject = _enemyContext.TargetObject;

        _rb = _gameObject.GetComponent<Rigidbody>();

        _radar = _enemyContext.Radar;

        _targetPositionField = _enemyContext.WaypointPosition;

        _lastCalculationTime = Time.time;
    }

    public void Process(ShipContext context)
    {
        if (_gameObject == null || _radar == null)
        {
            return;
        }
        if (_lastCalculationTime + CalculationInterval <= Time.time)
        {
            CalculateNewWaypoint();
            _lastCalculationTime = Time.time;
        }
    }

    public bool IsLocked() => false;

    private float CalculateReachingDifficulty(Vector3 selfPos, Vector3 point)
    {
        if (_rb == null)
        {
            return 0;
        }

        Vector3 linearVelocity = _rb.linearVelocity; linearVelocity.y = 0;
        Vector3 movementDirection = linearVelocity.normalized;
        Vector3 directionToTarget = (point - selfPos).normalized;
        float movementSpeed = linearVelocity.magnitude;

        float difficulty = (1 - Vector3.Dot(movementDirection, directionToTarget)) * movementSpeed;
        return difficulty;
    }

    private void CalculateNewWaypoint()
    {
        var targetTransform = _targetGameObject.Value == null? null: _targetGameObject.Value.transform;
        Vector3 selfPos = _gameObject.transform.position; selfPos.y = 0;

        var traces = _radar.Traces;

        List<Vector3> options = new();
        List<Vector3> directCollisions = new();
        List<float> movementDifficulty = new();
        List<float> distance = new();
        foreach (var trace in traces)
        {
            var node = trace.Node;
            if (node.Hits.Length > 0 && targetTransform != null)
            {
                var otherTransform = node.Hits[0].transform;
                if (otherTransform != null && (otherTransform == targetTransform || otherTransform.IsChildOf(targetTransform)))
                {
                    directCollisions.Add(node.Hits[0].point);
                }
                continue;
            }
            if (node.Hits.Length == 0)
            {
                Vector3 point = node.End;
                options.Add(point);

                movementDifficulty.Add(CalculateReachingDifficulty(selfPos, point));

                float totalDist = Vector3.Distance(selfPos, point) + Vector3.Distance(point, _destinationPosListener);
                distance.Add(totalDist);
            }
        }
        if (directCollisions.Count > 0)
        {
            int randIndex = Random.Range(0, directCollisions.Count - 1);
            _targetPositionField.Value = directCollisions[randIndex];
            return;
        }
        if (options.Count == 0)
        {
            _targetPositionField.Value = selfPos;
            return;
        }

        float minMovementDifficulty = movementDifficulty.Min(),
            maxMovementDifficulty = movementDifficulty.Max();

        float minDistance = distance.Min(),
            maxDistance = distance.Max(),
            distanceSpan = maxDistance - minDistance;

        Vector3 bestOption = options[0];
        float bestVal = movementDifficulty[0] * _difficultyCoef
            + (distance[0] - minDistance) / distanceSpan * _distanceCoef;

        for (int i = 1; i < options.Count; i++)
        {
            float candidateVal = movementDifficulty[i] * _difficultyCoef
            + (distance[i] - minDistance) / distanceSpan * _distanceCoef;

            if (candidateVal < bestVal)
            {
                bestVal = candidateVal;
                bestOption = options[i];
            }
        }

        _targetPositionField.Value = bestOption;
    }
}
