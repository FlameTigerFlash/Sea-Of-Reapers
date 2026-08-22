using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyweightPrefabsConfig", menuName = "Scriptable Objects/Prefab Configs/FlyweightPrefabsConfig")]
public class FlyweightPrefabsConfig : ScriptableObject
{
    [SerializeField] private GameObject _tracePrefab;

    public IReadOnlyDictionary<FlyweightTypes, GameObject> FlyweightPrefabs => _flyweightPrefabs;

    private Dictionary<FlyweightTypes, GameObject> _flyweightPrefabs = new();

    private void OnEnable()
    {
        _flyweightPrefabs[FlyweightTypes.PlayerCannonTrace] = _tracePrefab;
    }
}
