using Character.Enemy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.UI;
using UnityEngine;
using static Radar;

using Random = UnityEngine.Random;

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

    private Func<TraceNode, Vector3> GetFarthestReachablePoint = (node) =>
    {
        if (node.Hits == null || node.Hits.Length == 0)
        {
            return node.End;
        }
        return node.Hits[0].point;
    };

    private Func<Vector3, float> EstimatePointValue = null;

    private float _calculationInterval = 1f;
    private float _lastCalculationTime = 0f;
    #endregion

    public ObstacleAvoidanceStrategy(Func<TraceNode, Vector3> gfrp = null, Func<Vector3, float> epv = null)
    {
        if (gfrp != null)
        {
            GetFarthestReachablePoint = gfrp;
        }
        EstimatePointValue = epv;
        EstimatePointValue ??= (point) =>
        {
            Vector3 selfPos = _gameObject.transform.position; 
            selfPos.y = 0;
            float distSum = Vector3.Distance(selfPos, point) + Vector3.Distance(point, _destinationPosListener.Value);
            return (distSum == 0)? float.PositiveInfinity: 1 / distSum;
        };
    }

    public void Initialize(ShipContext context)
    {
        _enemyContext = context;

        SetupFields();
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

    private void SetupFields()
    {
        _destinationPosListener = _enemyContext.MovementDestination;

        _gameObject = _enemyContext.SelfObject;
        _targetGameObject = _enemyContext.TargetObject;

        _rb = _gameObject.GetComponent<Rigidbody>();

        _radar = _enemyContext.Radar;

        _targetPositionField = _enemyContext.WaypointPosition;
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

        var traces = _radar.Traces;

        List<Vector3> options = new();
        List<float> values = new();
        foreach (var trace in traces)
        {
            var node = trace.Node;

            Vector3 farthestPoint = GetFarthestReachablePoint(node);
            options.Add(farthestPoint);
            float value = EstimatePointValue(farthestPoint);
            values.Add(value);
        }
        if (options.Count == 0)
        {
            _targetPositionField.Value = _gameObject.transform.position;
            return;
        }

        Vector3 bestOption = options[0];
        float bestVal = values[0];

        for (int i = 1; i < options.Count; i++)
        {
            if (values[i] > bestVal)
            {
                bestVal = values[i];
                bestOption = options[i];
            }
        }

        _targetPositionField.Value = bestOption;
    }
}
