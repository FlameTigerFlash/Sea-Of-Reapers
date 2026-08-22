using UnityEngine;
using UnityEngine.Events;

public class ImpactHit : MonoBehaviour
{
    [SerializeField, Min(0)] private float _damage = 0;

    public UnityEvent HitEvent;

    private void OnTriggerEnter(Collider other)
    {
        var damageReceiver = other.gameObject.GetComponentInParent<ITakeDamage>();
        damageReceiver?.TakeDamage(_damage);

        HitEvent.Invoke();
        Destroy(gameObject);
    }
}
