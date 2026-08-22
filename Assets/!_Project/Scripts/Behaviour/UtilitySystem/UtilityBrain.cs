using Character.Enemy;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace UtilitySystem
{
    public class UtilityBrain : MonoBehaviour, IContextStrategy
    {
        [SerializeReference] private CommonStrategyAggregator _csa = new();

        [SerializeField] private ActionsConfig _actionsConfig;

        [SerializeField, Min(0f)] private float _decisionDelay = 3f;

        [SerializeField, Min(0f)] private float _decisionChangeThreshold = 0.05f;

        public CommonStrategyAggregator CSA => _csa;

        private ShipContext _context;

        private BaseAIAction _curAction = null;

        private List<BaseAIAction> _actions = new();

        private float _lastDecisionTime = float.NegativeInfinity;

        private void Awake()
        {
            _actions = _actionsConfig.GetActions();
        }

        public void Initialize(ShipContext context)
        {
            foreach (var action in _actions)
            {
                action.Initialize(context);
            }
            _csa.Initialize(context);
            _context = context;
            PickNewAction();
        }

        public void Process(ShipContext context)
        {
            if (Time.time - _lastDecisionTime >= _decisionDelay)
            {
                _lastDecisionTime = Time.time;
                PickNewAction();
            }
            _curAction?.Process(context);
            _csa.Process(context);
            //Debug.Log(_curAction);
        }

        private void PickNewAction()
        {
            if (_context == null)
            {
                return;
            }
            float bestVal = float.NegativeInfinity;
            BaseAIAction bestAction = null;

            foreach (var action in _actions)
            {
                float curVal = action.OnEvaluate(_context);
                if (action == _curAction)
                {
                    curVal += _decisionChangeThreshold;
                }
                if (curVal > bestVal)
                {
                    bestVal = curVal;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                _curAction = bestAction;
            }
        }
    }
}