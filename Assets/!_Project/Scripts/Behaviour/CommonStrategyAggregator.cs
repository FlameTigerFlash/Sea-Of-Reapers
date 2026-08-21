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

    public void Initialize(ShipContext context)
    {
        _context = context;

        _obstacleAvoidanceStrategy.Initialize(_context);
        _rotationStrategy.Initialize(_context);
        _waypointReachingStrategy.Initialize(_context);
        _defaultAimingStrategy.Initialize(_context);

        _aimingStrategy = _defaultAimingStrategy;
        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void Process(ShipContext context)
    {
        _pathfindingStrategy?.Process(context);
        _movementStrategy?.Process(context);
        _aimingStrategy?.Process(context);
    }

    public void ChaseTarget()
    {
        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void RotateTowardsTarget()
    {
        _pathfindingStrategy = null;
        _movementStrategy = _rotationStrategy;
    }
}
