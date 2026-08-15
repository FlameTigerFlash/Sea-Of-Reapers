using UnityEngine;

public class UtilityBrain
{
    private readonly EnemyContext _enemyContext;

    private readonly GameObject _gameObject;

    private readonly ReactiveListener<GameObject> _targetListener;
    private readonly ReactiveField<Vector3> _targetPositionField;

    private IStrategy _waypointReachingStrategy;
    private IStrategy _obstacleAvoidanceStrategy;
    private IStrategy _aimingStrategy;

    public UtilityBrain(EnemyContext context)
    {
        _enemyContext = context;

        _gameObject = _enemyContext.SelfObject;

        _targetListener = _enemyContext.TargetObject;
        _targetPositionField = _enemyContext.WaypointPosition;

        _waypointReachingStrategy = new WaypointReachingStrategy(); _waypointReachingStrategy.Initialize(context);
        _obstacleAvoidanceStrategy = new ObstacleAvoidanceStrategy(); _obstacleAvoidanceStrategy.Initialize(context);
        _aimingStrategy = new AimingStrategy(); _aimingStrategy.Initialize(context);
    }

    public void Decide()
    {
        if (_targetListener == null || _targetListener.Value == null)
        {
            return;
        }

        _obstacleAvoidanceStrategy.Update(_enemyContext);
        _waypointReachingStrategy.Update(_enemyContext);
        _aimingStrategy.Update(_enemyContext);
    }
}
