using Character.Enemy;
using UnityEngine;

public class WaypointReachingStrategy : IContextStrategy
{
    private ShipContext _context;

    private GameObject _gameObject;

    private ReactiveField<Vector3> _targetPositionField;
    private ReactiveField<float> _thrustField;
    private ReactiveField<float> _directionField;

    public void Initialize(ShipContext context)
    {
        _context = context;

        _gameObject = _context.SelfObject;

        _targetPositionField = _context.WaypointPosition;
        _thrustField = _context.ThrustMultiplier;
        _directionField = _context.ThrustDirection;
    }

    public void Process(ShipContext context)
    {
        if (_gameObject == null || _targetPositionField == null)
        {
            return;
        }
        Vector3 targetPos = _targetPositionField.Value, selfPos = _gameObject.transform.position;
        targetPos.y = 0;
        selfPos.y = 0;
        Vector3 connector = targetPos - selfPos, forwardVector = _gameObject.transform.forward;
        forwardVector.y = 0;

        float angle = Vector3.Angle(connector, forwardVector);
        Vector3 cross = Vector3.Cross(connector, forwardVector);
        angle *= Mathf.Sign(cross.y);

        float distance = connector.magnitude;
        if (distance <= 3)
        {
            return;
        }
        Vector3 direction = connector.normalized;

        float rotation = Mathf.Abs(angle) <= 5 ? 18 * angle : angle < 0 ? -90 : 90;

        _thrustField.Value = 1;
        _directionField.Value = rotation;
    }

    public bool IsLocked() => false;
}
