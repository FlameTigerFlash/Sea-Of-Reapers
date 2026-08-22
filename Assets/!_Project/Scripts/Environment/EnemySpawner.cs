using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Collections;
using Character.Enemy;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private IFactory<EnemyTypes, Vector3, Quaternion, EnemyContextHandler> _enemyFactory;

    [Inject]
    public void Construct(IFactory<EnemyTypes, Vector3, Quaternion, EnemyContextHandler> enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public GameObject SpawnEnemy()
    {
        return SpawnEnemy(EnemyTypes.Corvette);
    }

    public GameObject SpawnEnemy(EnemyTypes enemyType)
    {
        var enemyContextHandler = _enemyFactory.Create(enemyType, _spawnPoint.position, _spawnPoint.rotation);
        enemyContextHandler.SetActive();

        return enemyContextHandler.gameObject;
    }
}
