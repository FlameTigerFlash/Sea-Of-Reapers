using UnityEngine;

public interface IStrategy
{
    public void Initialize(EnemyContext context);

    public void Process(EnemyContext context);

    public bool IsLocked() => false;
}
