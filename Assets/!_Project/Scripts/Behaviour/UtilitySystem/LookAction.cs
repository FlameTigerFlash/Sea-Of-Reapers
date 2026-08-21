using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class LookAction : BaseAIAction
    {
        private ShipContext _context;

        public override void Initialize(ShipContext context)
        {
            _context = context;

        }

        public override void Process(ShipContext context)
        {
            context.Brain.CSA.RotateTowardsTarget();
        }
    }
}