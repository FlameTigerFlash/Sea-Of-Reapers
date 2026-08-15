using UnityEngine;

public interface IStrategy
{
    public void Initialize(EnemyContext context);

    public void Update(EnemyContext context);

    public bool IsLocked() => false;
}
