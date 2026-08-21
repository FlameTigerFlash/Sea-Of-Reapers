using Character.Enemy;
using System;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class ChaseAction : BaseAIAction
    {
        private ShipContext _context;

        public override void Initialize(ShipContext context)
        {
            _context = context;
        }

        public override void Process(ShipContext context)
        {
            context.Brain.CSA.ChaseTarget();
        }
    }
}