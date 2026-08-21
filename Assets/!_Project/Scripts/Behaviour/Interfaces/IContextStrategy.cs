using Character.Enemy;
using UnityEngine;

public interface IContextStrategy
{
    public void Initialize(ShipContext context);

    public void Process(ShipContext context);

    public bool IsLocked() => false;
}
