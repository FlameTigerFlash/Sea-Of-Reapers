using UnityEngine;

public class ImpactHit : MonoBehaviour
{
    [SerializeField, Min(0)] private float _damage = 0;

    private void OnTriggerEnter(Collider other)
    {
        var damageReceiver = other.gameObject.GetComponentInParent<ITakeDamage>();
        if (damageReceiver != null)
        {
            damageReceiver.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}
