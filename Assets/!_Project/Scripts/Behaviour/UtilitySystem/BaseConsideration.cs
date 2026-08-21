using UnityEngine;
using System;
using Character.Enemy;

namespace UtilitySystem
{
    [Serializable]
    public abstract class BaseConsideration
    {
        public float OnEvaluate(ShipContext context)
        {
            return Mathf.Clamp01(Evaluate(context));
        }

        protected abstract float Evaluate(ShipContext context);
    }
}