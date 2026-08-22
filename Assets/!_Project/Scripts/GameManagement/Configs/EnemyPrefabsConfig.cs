using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "EnemyPrefabsConfig", menuName = "Scriptable Objects/EnemyPrefabsConfig")]
public class EnemyPrefabsConfig : ScriptableObject
{
    [SerializeField] private GameObject _gunboatPrefab;
    [SerializeField] private GameObject _corvettePrefab;

    public IReadOnlyDictionary<EnemyTypes, GameObject> EnemyPrefabs => _enemyPrefabs;

    private Dictionary<EnemyTypes, GameObject> _enemyPrefabs = new();

    private void OnEnable()
    {
        _enemyPrefabs[EnemyTypes.Gunboat] = _gunboatPrefab;
        _enemyPrefabs[EnemyTypes.Corvette] = _corvettePrefab;
    }
}
