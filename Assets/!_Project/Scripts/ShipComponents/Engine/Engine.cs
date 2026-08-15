using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField, NotNull] private Rigidbody _rb;

    [SerializeField, NotNull] private Transform _thrustPoint;

    [SerializeField, Min(0)] private float _maxForwardThrust = 100000;
    [SerializeField, Min(0)] private float _maxBackwardThrust = 50000;

    public float MaxForwardThrust => _maxForwardThrust;
    public float MaxBackwardThrust => _maxBackwardThrust;
    public float Thrust 
    {
        get => _thrust;
        set
        {
            float newThrust = Mathf.Clamp(value, -_maxBackwardThrust, _maxForwardThrust);
            _thrust = newThrust;
        }
    }

    private float _thrust = 0;

    private void OnValidate()
    {
        if (_rb == null)
        {
            _rb = GetComponentInParent<Rigidbody>();
        }
        if (_thrustPoint == null)
        {
            _thrustPoint = transform;
        }
    }

    private void FixedUpdate()
    {
        Vector3 forceVector = _thrustPoint.forward * Thrust;
        _rb.AddForceAtPosition(forceVector, _thrustPoint.position);
    }

    public void SetThrustByMultiplier(float multiplier)
    {
        multiplier = Mathf.Clamp(multiplier, -1, 1);

        if (multiplier >= 0)
        {
            Thrust = _maxForwardThrust * multiplier;
        }
        else
        {
            Thrust = _maxBackwardThrust * multiplier;
        }
    }
}
