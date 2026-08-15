using UnityEngine;
using UnityEngine.Events;

public class DeathHandler : MonoBehaviour
{
    public UnityEvent DeathEvent;

    protected bool _isDead = false;

    public void OnHandleDeath()
    {
        if (_isDead)
        {
            return;
        }

        HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        DeathEvent.Invoke();
        Destroy(gameObject);
    }
}
