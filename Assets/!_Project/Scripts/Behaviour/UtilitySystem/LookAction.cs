using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class LookAction : BaseAIAction
{
    private EnemyContext _enemyContext;

    private ReactiveListener<GameObject> _targetListener;

    private IStrategy _aimingStrategy;
    private IStrategy _rotationStraregy;

    public override void Initialize(EnemyContext context)
    {
        _enemyContext = context;

        _targetListener = _enemyContext.TargetObject;

        _aimingStrategy = new AimingStrategy(); _aimingStrategy.Initialize(context);
        _rotationStraregy = new RotationStrategy(); _rotationStraregy.Initialize(context);
    }

    public override void Process(EnemyContext context)
    {
        if (_targetListener == null || _targetListener.Value == null)
        {
            return;
        }
        _rotationStraregy.Process(context);
        _aimingStrategy.Process(context);

    }
}
