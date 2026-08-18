using UnityEngine;
using System;

[Serializable]
public abstract class BaseConsideration
{
    public float OnEvaluate(EnemyContext context)
    {
        return Mathf.Clamp01(Evaluate(context));
    }

    protected abstract float Evaluate(EnemyContext context);
}
