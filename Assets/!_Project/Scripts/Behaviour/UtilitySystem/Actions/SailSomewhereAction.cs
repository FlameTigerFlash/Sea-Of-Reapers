using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class SetCustomDestinationAction : BaseAIAction
    {
        [SerializeField] private Vector3 _destination;

        private ShipContext _context;

        public override void Initialize(ShipContext context)
        {
            _context = context;

        }

        public override void Process(ShipContext context)
        {
            context.Brain.CSA.SetAutopilot(true);
            context.Brain.CSA.SetDestination(_destination);
        }
    }
}
