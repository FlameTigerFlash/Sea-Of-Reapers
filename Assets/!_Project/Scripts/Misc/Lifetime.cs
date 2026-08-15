using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float _lifetime = 20f;

    [SerializeField] private bool _autoDestruct;

    public UnityEvent TimeOverEvent;

    private void Awake()
    {
        StartCoroutine(LifetimeCoroutine(_lifetime));
    }

    public void SetLifetime(float lifetime)
    {
        _lifetime = lifetime;
        StopAllCoroutines();
        StartCoroutine(LifetimeCoroutine(_lifetime));
    }

    private void HandleTimeOver()
    {
        TimeOverEvent.Invoke();
        if (_autoDestruct )
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator LifetimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        HandleTimeOver();
    }
}
