using UnityEngine;

public class EngineController : IUpdateControls
{
    private readonly Engine _engine;
    private readonly IRotate _rotor;

    private readonly ReactiveListener<float> _multiplier;
    private readonly ReactiveListener<float> _direction;

    public EngineController(Engine engine, IRotate rotor, ReactiveListener<float> multiplier, ReactiveListener<float> direction)
    {
        _engine = engine;
        _rotor = rotor;
        _multiplier = multiplier;
        _direction = direction;
    }

    public void UpdateControls()
    {
        _engine?.SetThrustByMultiplier(_multiplier.Value);
        _rotor?.RotateLocally(Quaternion.Euler(0, _direction.Value, 0));
    }

    public void HaltControls()
    {
        _engine?.SetThrustByMultiplier(0);
        _rotor?.RotateLocally(Quaternion.Euler(0, 0, 0));
    }
}
