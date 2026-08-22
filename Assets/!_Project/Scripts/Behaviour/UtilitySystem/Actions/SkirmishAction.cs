using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class SkirmishAction : BaseAIAction
    {
        [SerializeReference] private SimpleDistanceEvaluator _distanceEvaluator = new();
        [SerializeReference] private SimpleAngleEvaluator _angleEvaluator = new();

        [SerializeField, Range(0, 1)] private float _distanceCoefficient = 1f;
        [SerializeField, Range(0, 1)] private float _angleCoefficient = 1f;
        [SerializeField, Range(0, 1)] private float _rotationCoefficient = 1f;

        private ShipContext _context;

        private IContextStrategy _skirmishMovementStrategy;

        public override void Initialize(ShipContext context)
        {
            _context = context;

            _skirmishMovementStrategy = new ObstacleAvoidanceStrategy(null, EvaluatePoint);
            _skirmishMovementStrategy.Initialize(context);
        }

        public override void Process(ShipContext context)
        {
            if (_context.TargetObject.Value == null)
            {
                return;
            }

            context.Brain.CSA.SetAutopilot(false);
            context.Brain.CSA.ChaseTarget();
            _skirmishMovementStrategy.Process(context);
        }

        private float EvaluatePoint(Vector3 point)
        {
            Vector3 selfPos = _context.SelfObject.transform.position; selfPos.y = 0;
            Vector3 selfForward = _context.SelfObject.transform.forward;
            Vector3 enemyObjectPos = _context.TargetObject.Value.transform.position; enemyObjectPos.y = 0;
            Vector3 pointCon = point - selfPos, enemyCon = enemyObjectPos - point;

            float angle = 0.5f * Vector3.Angle(pointCon, enemyCon) + 0.5f * Vector3.Angle(selfForward, (enemyObjectPos - selfPos));
            float distanceEval = _distanceEvaluator.Evaluate(Vector3.Distance(point, enemyObjectPos)),
                angleEval = _angleEvaluator.Evaluate(angle);

            float rotationEval = (1 + Vector3.Dot(selfForward, pointCon.normalized)) / 2;

            //Debug.Log($"Distance eval: {distanceEval}, angle eval: {angleEval}, rotation eval: {rotationEval}.");

            return distanceEval * _distanceCoefficient + angleEval * _angleCoefficient + rotationEval * _rotationCoefficient;
        }
    }
}