using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private IFactory<Vector3, Quaternion, EnemyContextHandler> _enemyFactory;

    [Inject]
    public void Construct(IFactory<Vector3, Quaternion, EnemyContextHandler> enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public GameObject SpawnEnemy()
    {
        var enemyContextHandler = _enemyFactory.Create(_spawnPoint.position, _spawnPoint.rotation);
        enemyContextHandler.SetActive();

        return enemyContextHandler.gameObject;
    }
}
