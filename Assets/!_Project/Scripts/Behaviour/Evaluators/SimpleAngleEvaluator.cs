using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SimpleAngleEvaluator
{
    [SerializeField, Range(0, 180)] private float _forwardArc = 90;
    [SerializeField, Range(0, 180)] private float _rearArc = 90;
    [SerializeField, Range(0, 1)] private float _forwardValue = 1f;
    [SerializeField, Range(0, 1)] private float _sideValue = 0.5f;
    [SerializeField, Range(0, 1)] private float _rearValue = 0f;

    public float Evaluate(float angle)
    {
        angle = Mathf.DeltaAngle(0, angle);
        float absAngle = Mathf.Abs(angle);

        if (absAngle <= _forwardArc / 2)
        {
            return _forwardValue;
        }
        else if (180 - absAngle <= _rearArc)
        {
            return _rearValue;
        }
        return _sideValue;
    }
}
