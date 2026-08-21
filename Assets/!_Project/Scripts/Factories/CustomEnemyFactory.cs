using Character;
using Character.Enemy;
using UnityEngine;
using Zenject;

public class CustomEnemyFactory : IFactory<Vector3, Quaternion, EnemyContextHandler>
{
    private DiContainer _container;
    private MapLocator _mapLocator;
    private GameObject _enemyPrefab;

    [Inject]
    public CustomEnemyFactory(DiContainer container, MapLocator locator, GameObject enemyPrefab)
    {
        _container = container;
        _enemyPrefab = enemyPrefab;
        _mapLocator = locator;
    }

    public EnemyContextHandler Create(Vector3 position, Quaternion rotation)
    {
        EnemyContextHandler contextHandler = null;
        if (_mapLocator == null || _mapLocator.EnemiesFolder == null)
        {
            contextHandler = _container.InstantiatePrefabForComponent<EnemyContextHandler>(_enemyPrefab, position, rotation, null);
        }
        else
        {
            contextHandler = _container.InstantiatePrefabForComponent<EnemyContextHandler>(_enemyPrefab, position, rotation, _mapLocator.EnemiesFolder);
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
