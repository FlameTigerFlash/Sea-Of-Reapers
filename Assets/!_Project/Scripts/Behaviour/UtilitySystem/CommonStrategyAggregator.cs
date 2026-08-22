using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class CommonStrategyAggregator : IContextStrategy
{
    public bool IsChasing { get; private set; } = false;
    public bool IsOnAutopilot { get; private set; } = true;
    public bool IsMoving { get; private set; } = true;
    public bool IsAiming { get; private set; } = true;

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
    }

    public void Process(ShipContext context)
    {
        if (IsChasing && _context.TargetObject.Value != null)
        {
            _context.MovementDestination.Value = _context.TargetObject.Value.transform.position;
        }
        if (IsOnAutopilot)
        {
            _pathfindingStrategy?.Process(context);
        }
        if (IsMoving)
        {
            _movementStrategy?.Process(context);
        }
        if (IsAiming)
        {
            _aimingStrategy?.Process(context);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        IsChasing = false;
        IsMoving = true;
        _context.MovementDestination.Value = destination;

        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void ChaseTarget()
    {
        IsChasing = true;
        IsMoving = true;
        _movementStrategy = _waypointReachingStrategy;
        _pathfindingStrategy = _obstacleAvoidanceStrategy;
    }

    public void RotateTowardsTarget()
    {
        IsMoving = false;
        _pathfindingStrategy = null;
        _movementStrategy = _rotationStrategy;
    }

    public void StopMoving()
    {
        IsChasing = false;
        IsMoving = false;
    }

    public void SetAutopilot(bool autopilot)
    {
        IsOnAutopilot = autopilot;
    }
}
