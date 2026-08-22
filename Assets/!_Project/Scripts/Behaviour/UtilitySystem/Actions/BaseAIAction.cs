using UnityEngine;
using System;
using Character.Enemy;

namespace UtilitySystem
{
    [Serializable]
    public abstract class BaseAIAction : IContextStrategy, ICloneable
    {
        [SerializeReference, SubclassSelector] private BaseConsideration _consideration;

        public BaseAIAction CreateShallowCopy()
        {
            return (BaseAIAction)Clone();
        }

        public virtual float OnEvaluate(ShipContext context) => _consideration.OnEvaluate(context);

        public virtual void Initialize(ShipContext context)
        {

        }

        public abstract void Process(ShipContext context);

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}