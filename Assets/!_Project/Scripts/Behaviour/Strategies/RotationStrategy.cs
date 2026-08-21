using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RotationStrategy : IContextStrategy
{
    private ShipContext _enemyContext;

    private GameObject _gameObject;

    private ReactiveListener<Vector3> _destinationPosListener;

    private ReactiveField<float> _thrustField;
    private ReactiveField<float> _directionField;

    public void Initialize(ShipContext context)
    {
        _enemyContext = context;

        _gameObject = _enemyContext.SelfObject;

        _destinationPosListener = _enemyContext.MovementDestination;
        _thrustField = _enemyContext.ThrustMultiplier;
        _directionField = _enemyContext.ThrustDirection;
    }

    public void Process(ShipContext context)
    {
        Vector3 targetPos = _destinationPosListener.Value, selfPos = _gameObject.transform.position;
        targetPos.y = 0;
        selfPos.y = 0;
        Vector3 connector = targetPos - selfPos, forwardVector = _gameObject.transform.forward;
        forwardVector.y = 0;

        float angle = Vector3.Angle(connector, forwardVector);
        Vector3 cross = Vector3.Cross(connector, forwardVector);
        angle *= Mathf.Sign(cross.y);

        Vector3 direction = connector.normalized;

        float rotation = Mathf.Abs(angle) <= 5 ? 18 * angle : angle < 0 ? -90 : 90;

        _thrustField.Value = Mathf.Min(1, Mathf.Abs(rotation) / 15);
        _directionField.Value = rotation;
    }

    public bool IsLocked() => false;
}
