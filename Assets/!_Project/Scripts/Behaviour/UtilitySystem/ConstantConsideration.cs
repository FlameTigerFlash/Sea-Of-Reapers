using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class ConstantConsideration : BaseConsideration
{
    [SerializeField, Range(0f, 1f)] private float _value = 0f;

    protected override float Evaluate(EnemyContext context)
    {
        return _value;
    }
}
