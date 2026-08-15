using NUnit.Framework;
using UnityEngine;

public class AimingStrategy : IStrategy
{
    private EnemyContext _enemyContext;

    private GameObject _selfObject;
    private Cannon _cannon;

    private ReactiveListener<GameObject> _targetObject;

    private float _hitRange = 4f;

    public void Initialize(EnemyContext context)
    {
        _enemyContext = context;

        _selfObject = _enemyContext.SelfObject;
        _targetObject = _enemyContext.TargetObject;
        _cannon = _enemyContext.Cannon;
    }

    public void Update(EnemyContext context)
    {
        Vector3 targetPos = _targetObject.Value.transform.position;
        Vector3 shootDirection = _cannon.AimToTarget(targetPos);
        if (shootDirection == Vector3.zero)
        {
            return;
        }
        _cannon.RotateTowards(shootDirection);

        if (_cannon.CanShoot && ValidateShootingTrajectory())
        {
            _cannon.TryShoot();
        }
    }

    private bool ValidateShootingTrajectory()
    {
        var tracesList = _cannon.GetTrace(_cannon.GlobalRotation * Vector3.forward);
        if (tracesList == null || tracesList.Count == 0)
        {
            return false;
        }

        var trace = tracesList[0];
        foreach (var node in trace)
        {
            var hits = node.Hits;
            if (hits != null && hits.Length != 0)
            {
                Rigidbody otherRb = hits[0].rigidbody;
                if (otherRb != null && hits[0].rigidbody.gameObject == _targetObject.Value)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }

            Vector3 end = node.End;
            float minDist = Vector3.Distance(_targetObject.Value.transform.position, end);
            if (minDist <= _hitRange)
            {
                return true;
            }
        }

        return false;
    }
}
