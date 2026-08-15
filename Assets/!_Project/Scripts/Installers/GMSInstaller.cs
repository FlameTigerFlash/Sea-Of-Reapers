using UnityEngine;
using Zenject;
using System.Diagnostics.CodeAnalysis;

public class GMSInstaller : MonoInstaller
{
    [SerializeField, NotNull] private GameManager _gameManager;
    [SerializeField, NotNull] private MapLocator _mapLocator;
    [SerializeField, NotNull] private ScoreManager _scoreManager;
    [SerializeField, NotNull] private WaveManager _waveManager;

    private void OnValidate()
    {
        _gameManager = _gameManager != null ? _gameManager : GetComponentInChildren<GameManager>();
        _mapLocator = _mapLocator != null ? _mapLocator : GetComponentInChildren<MapLocator>();
        _scoreManager = _scoreManager != null ? _scoreManager : GetComponentInChildren<ScoreManager>();
        _waveManager = _waveManager != null ? _waveManager : GetComponentInChildren<WaveManager>();
    }

    public override void InstallBindings()
    {
        Container.BindInstance(_gameManager);
        Container.BindInstance(_mapLocator);
        Container.BindInstance(_scoreManager);
        Container.BindInstance(_waveManager);
    }
}