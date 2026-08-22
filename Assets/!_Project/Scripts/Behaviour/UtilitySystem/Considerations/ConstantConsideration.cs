using Character.Enemy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UtilitySystem
{
    [Serializable]
    public class ConstantConsideration : BaseConsideration
    {
        [SerializeField, Range(0f, 1f)] private float _value = 0f;

        protected override float Evaluate(ShipContext context)
        {
            return _value;
        }
    }
}