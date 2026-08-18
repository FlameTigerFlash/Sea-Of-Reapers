using UnityEngine;

public class WaypointReachingStrategy : IStrategy
{
    private EnemyContext _enemyContext;

    private GameObject _gameObject;

    private ReactiveField<Vector3> _targetPositionField;
    private ReactiveField<float> _thrustField;
    private ReactiveField<float> _directionField;

    public void Initialize(EnemyContext context)
    {
        _enemyContext = context;

        _gameObject = _enemyContext.SelfObject;

        _targetPositionField = _enemyContext.WaypointPosition;
        _thrustField = _enemyContext.ThrustMultiplier;
        _directionField = _enemyContext.ThrustDirection;
    }

    public void Process(EnemyContext context)
    {
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
