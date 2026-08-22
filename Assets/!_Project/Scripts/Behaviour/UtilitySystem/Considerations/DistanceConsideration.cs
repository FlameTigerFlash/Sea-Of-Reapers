using Character.Enemy;
using System;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class DistanceConsideration : BaseConsideration
    {
        [SerializeReference] private SimpleDistanceEvaluator _distanceEvaluator = new();

        protected override float Evaluate(ShipContext context)
        {
            if (context.TargetObject.Value == null)
            {
                return 0;
            }

            Vector3 selfPos = context.SelfObject.transform.position;
            Vector3 targetPos = context.TargetObject.Value.transform.position;

            float distance = Vector3.Distance(selfPos, targetPos);
            return Mathf.Clamp01(_distanceEvaluator.Evaluate(distance));
        }
    }
}