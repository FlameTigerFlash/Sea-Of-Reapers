using UnityEngine;
using Zenject;

public class DummyController : MonoBehaviour
{
    public void OnHealthChanged(float currentHealth)
    {
        Debug.Log($"Dummy HP has changed to {currentHealth}.");
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
