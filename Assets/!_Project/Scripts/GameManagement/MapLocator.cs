using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocator : MonoBehaviour
{
    [SerializeField] private Transform _enemiesFolder;

    [SerializeField] private float _cleanupDelay = 100f;

    public event Action<GameObject, bool> EnemyDataChangedEvent;

    public Transform EnemiesFolder => _enemiesFolder;

    public IReadOnlyCollection<GameObject> EnemiesCollection => _enemies;

    public ReactiveListener<GameObject> PlayerListener
    {
        get
        {
            _playerListener ??= new(_playerField);
            return _playerListener;
        }
    }

    private readonly ReactiveField<GameObject> _playerField = new();

    private ReactiveListener<GameObject> _playerListener;

    private readonly HashSet<GameObject> _enemies = new();

    private void Start()
    {
        _playerField.Value = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CleanupCoroutine(_cleanupDelay));
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
        EnemyDataChangedEvent?.Invoke(enemy, true);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        EnemyDataChangedEvent?.Invoke(enemy, false);
    }

    private IEnumerator CleanupCoroutine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            _enemies.RemoveWhere((GameObject obj) => obj == null);
        }
    }
}
