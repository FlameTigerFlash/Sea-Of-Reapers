using UnityEngine;
using System;

[Serializable]
public abstract class BaseAIAction : IStrategy, ICloneable
{
    [SerializeReference, SubclassSelector] private BaseConsideration _consideration;

    public BaseAIAction CreateShallowCopy()
    {
        return (BaseAIAction)Clone();
    }

    public virtual float OnEvaluate(EnemyContext context) => _consideration.OnEvaluate(context);

    public virtual void Initialize(EnemyContext context)
    {

    }

    public abstract void Process(EnemyContext context);

    public object Clone()
    {
        return MemberwiseClone();
    }
}
