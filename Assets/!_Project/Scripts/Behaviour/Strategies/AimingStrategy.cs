using Character.Enemy;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class AimingStrategy : IContextStrategy
{
    #region fields
    private ShipContext _enemyContext;

    private GameObject _selfObject;
    private List<Cannon> _cannons;

    private ReactiveListener<GameObject> _targetObject;

    private Vector3 _shootDirection = Vector3.forward;

    private float _hitRange = 4f;

    private float _maxAngleOffset = 1f;
    #endregion

    public void Initialize(ShipContext context)
    {
        _enemyContext = context;

        _selfObject = _enemyContext.SelfObject;
        _targetObject = _enemyContext.TargetObject;
        _cannons = _enemyContext.Cannons;
    }

    public void Process(ShipContext context)
    {
        if (_targetObject == null || _targetObject.Value == null)
        {
            return;
        }
        
        foreach (var cannon in _cannons)
        {
            HandleCannonAiming(cannon);
        }
    }

    private void HandleCannonAiming(Cannon cannon)
    {
        Vector3 targetPos = _targetObject.Value.transform.position;
        _shootDirection = cannon.AimToTarget(targetPos, out var canHit);
        if (_shootDirection == Vector3.zero || !canHit) return;

        cannon.RotateTowards(_shootDirection);

        if (cannon.CanShoot && ValidateShootingTrajectory(cannon))
        {
            cannon.TryShoot();
        }
    }

    private bool ValidateShootingTrajectory(Cannon cannon)
    {
        Vector3 cannonDir = cannon.GlobalRotation * Vector3.forward;

        var tracesList = cannon.GetTrace(cannon.GlobalRotation * Vector3.forward);
        if (tracesList == null || tracesList.Count == 0) return false;

        var trace = tracesList[0];
        foreach (var node in trace)
        {
            var hits = node.Hits;
            if (hits != null && hits.Length != 0)
            {
                bool targetHit = hits[0].transform.IsChildOf(_targetObject.Value.transform);
                if (targetHit) return true;
                continue;
            }

            Vector3 end = node.End;
            float minDist = Vector3.Distance(_targetObject.Value.transform.position, end);
            if (minDist <= _hitRange) return true;
        }
        if (Mathf.Abs(Vector3.Angle(cannonDir, _shootDirection)) <= _maxAngleOffset) return true;

        return false;
    }
}
