using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

public class ParticlesDecoupler : MonoBehaviour
{
    [SerializeField, NotNull] private ParticleSystem _particleSystem;

    private Transform _folder = null;

    private void OnValidate()
    {
        _particleSystem = _particleSystem != null ? _particleSystem : GetComponentInChildren<ParticleSystem>();
    }

    [Inject]
    public void Construct(MapLocator _mapLocator)
    {
        _folder = _mapLocator.ParticlesFolder;
    }

    public void OnDecouple()
    {
        transform.SetParent(_folder);
        _particleSystem.Stop();
    }

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
