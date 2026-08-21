using Character.Enemy;
using System;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class DistanceConsideration : BaseConsideration
    {
        [SerializeField, Min(0f)] private float _minDistance = 0;
        [SerializeField, Min(0f)] private float _maxDistance = 0;

        [SerializeField] private AnimationCurve _curve;

        public DistanceConsideration()
        {
            _maxDistance = Mathf.Max(_maxDistance, _minDistance + 0.1f);
        }

        protected override float Evaluate(ShipContext context)
        {
            if (context.TargetObject.Value == null)
            {
                return 0;
            }

            Vector3 selfPos = context.SelfObject.transform.position;
            Vector3 targetPos = context.TargetObject.Value.transform.position;

            float distance = Vector3.Distance(selfPos, targetPos);
            if (distance <= _minDistance)
            {
                Mathf.Clamp01(_curve.Evaluate(0));
            }

            distance = Mathf.Min(_maxDistance, distance);
            float ratio = (distance - _minDistance) / (_maxDistance - _minDistance);
            return Mathf.Clamp01(_curve.Evaluate(ratio));
        }
    }
}