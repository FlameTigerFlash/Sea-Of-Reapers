using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class CommonStrategyAggregator : IContextStrategy
{
    private ShipContext _context;

    private IContextStrategy _pathfindingStrategy;
    private IContextStrategy _movementStrategy;
    private IContextStrategy _aimingStrategy;

    private readonly IContextStrategy _obstacleAvoidanceStrategy = new ObstacleAvoidanceStrategy();
    private readonly IContextStrategy _rotationStrategy = new RotationStrategy();
    private readonly IContextStrategy _waypointReachingStrategy = new WaypointReachingStrategy();
    private readonly IContextStrategy _defaultAimingStrategy = new AimingStrategy();

    private bool _isChasing = false;

    public void Initialize(ShipContext context)
    {
        _context = context;

        _obstacleAvoidanceStrategy.Initialize(_context);
        _rotationStrategy.Initialize(_context);
        _waypointReachingStrategy.Initialize(_context);
        _defaultAimingStrategy.Initialize(_context);

        _aimingStrategy = _defaultAimingStrategy;
    }

    public void Process(ShipContext context)
    {
        if (_isChasing && _context.TargetObject.Value != null)
        {
            _context.MovementDestination.Value = _context.TargetObject.Value.transform.position;
        }

        _pathfindingStrategy?.Process(context);
        _movementStrategy?.Process(context);
        _aimingStrategy?.Process(context);
    }

    public void SetDestination(Vector3 destination)
    {
        _isChasing = false;
        _context.MovementDestination.Value = destination;

        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void ChaseTarget()
    {
        _isChasing = true;

        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void RotateTowardsTarget()
    {
        _pathfindingStrategy = null;
        _movementStrategy = _rotationStrategy;
    }

    public void StopMoving()
    {
        _isChasing = false;
        _movementStrategy = null;
        _pathfindingStrategy = null;
    }
}
