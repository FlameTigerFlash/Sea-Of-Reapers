using Character;
using Character.Enemy;
using System;
using UnityEngine;
using Zenject;

public class CustomEnemyFactory : IFactory<EnemyTypes, Vector3, Quaternion, EnemyContextHandler>
{
    private DiContainer _container;
    private MapLocator _mapLocator;
    private EnemyPrefabsConfig _prefabsConfig;

    [Inject]
    public CustomEnemyFactory(DiContainer container, MapLocator locator, EnemyPrefabsConfig prefabsConfig)
    {
        _container = container;
        _prefabsConfig = prefabsConfig;
        _mapLocator = locator;
    }

    public EnemyContextHandler Create(EnemyTypes enemyType, Vector3 position, Quaternion rotation)
    {
        if (!_prefabsConfig.EnemyPrefabs.ContainsKey(enemyType))
        {
            throw new ArgumentException($"Enemy type: {enemyType} does not provide a prefab to spawn.");
        }
        GameObject enemyPrefab = _prefabsConfig.EnemyPrefabs[enemyType];

        EnemyContextHandler contextHandler = null;
        if (_mapLocator == null || _mapLocator.EnemiesFolder == null)
        {
            contextHandler = _container.InstantiatePrefabForComponent<EnemyContextHandler>(enemyPrefab, position, rotation, null);
        }
        else
        {
            contextHandler = _container.InstantiatePrefabForComponent<EnemyContextHandler>(enemyPrefab, position, rotation, _mapLocator.EnemiesFolder);
        }

        if (_mapLocator != null)
        {
            _mapLocator.AddEnemy(contextHandler.gameObject);

            DeathHandler deathHandler = contextHandler.gameObject.GetComponentInChildren<DeathHandler>();
            if (deathHandler == null)
            {
                Debug.LogError("Spawned enemy must have a death handler component.");
            }
            deathHandler.DeathEvent.AddListener(() => _mapLocator.RemoveEnemy(deathHandler.gameObject));
        }
        else
        {
            Debug.LogWarning("Map locator was not bound.");
        }

        return contextHandler;
    }
}
