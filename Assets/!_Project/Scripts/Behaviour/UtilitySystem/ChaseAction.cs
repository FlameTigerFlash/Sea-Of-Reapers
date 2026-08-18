using System;
using UnityEngine;

[Serializable]
public class ChaseAction : BaseAIAction
{
    private EnemyContext _enemyContext;

    private GameObject _gameObject;

    private ReactiveListener<GameObject> _targetListener;

    private ReactiveField<Vector3> _targetPositionField;

    private IStrategy _waypointReachingStrategy;
    private IStrategy _obstacleAvoidanceStrategy;
    private IStrategy _aimingStrategy;

    public override void Initialize(EnemyContext context)
    {
        _enemyContext = context;

        _gameObject = _enemyContext.SelfObject;

        _targetListener = _enemyContext.TargetObject;
        _targetPositionField = _enemyContext.WaypointPosition;

        _waypointReachingStrategy = new WaypointReachingStrategy(); _waypointReachingStrategy.Initialize(context);
        _obstacleAvoidanceStrategy = new ObstacleAvoidanceStrategy(); _obstacleAvoidanceStrategy.Initialize(context);
        _aimingStrategy = new AimingStrategy(); _aimingStrategy.Initialize(context);
    }

    public override void Process(EnemyContext context)
    {
        if (_targetListener == null || _targetListener.Value == null)
        {
            return;
        }

        _obstacleAvoidanceStrategy.Process(_enemyContext);
        _waypointReachingStrategy.Process(_enemyContext);
        _aimingStrategy.Process(_enemyContext);
    }
}
