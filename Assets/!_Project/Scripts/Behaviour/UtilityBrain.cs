using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBrain : MonoBehaviour, IStrategy
{
    [SerializeField] private ActionsConfig _actionsConfig;

    [SerializeField, Min(0f)] private float _decisionDelay = 3f;

    private EnemyContext _context;

    private BaseAIAction _curAction = null;

    private List<BaseAIAction> _actions = new();

    private float _lastDecisionTime = float.NegativeInfinity;

    private void Awake()
    {
        _actions = _actionsConfig.GetActions();
    }

    public void Initialize(EnemyContext context)
    {
        foreach (var action in _actions)
        {
            action.Initialize(context);
        }
        _context = context;
        PickNewAction();
    }

    public void Process(EnemyContext context)
    {
        if (Time.time - _lastDecisionTime >= _decisionDelay)
        {
            _lastDecisionTime = Time.time;
            PickNewAction();
        }
        _curAction?.Process(context);
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
