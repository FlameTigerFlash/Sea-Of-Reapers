using System;
using UnityEngine;

[Serializable]
public class SimpleDistanceEvaluator
{
    [SerializeField, Min(0f)] private float _minDistance = 0;
    [SerializeField, Min(0f)] private float _maxDistance = 100;

    [SerializeField] private AnimationCurve _curve;

    public SimpleDistanceEvaluator()
    {
        _maxDistance = Mathf.Max(_maxDistance, _minDistance + 0.1f);
    }

    public float Evaluate(float distance)
    {
        if (distance <= _minDistance)
        {
            Mathf.Clamp01(_curve.Evaluate(0));
        }

        distance = Mathf.Min(_maxDistance, distance);
        float ratio = (distance - _minDistance) / (_maxDistance - _minDistance);
        return Mathf.Clamp01(_curve.Evaluate(ratio));
    }
}
