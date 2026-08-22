using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectilePrefabsConfig", menuName = "Scriptable Objects/Prefab Configs/ProjectilePrefabsConfig")]
public class ProjectilePrefabsConfig : ScriptableObject
{
    [SerializeField] private GameObject _blueBallPrefab;
    [SerializeField] private GameObject _redBallPrefab;

    public IReadOnlyDictionary<ProjectileTypes, GameObject> ProjectilePrefabs => _projectilePrefabs;

    private Dictionary<ProjectileTypes, GameObject> _projectilePrefabs = new();

    private void OnEnable()
    {
        _projectilePrefabs[ProjectileTypes.BlueBall] = _blueBallPrefab;
        _projectilePrefabs[ProjectileTypes.RedBall] = _redBallPrefab;
    }
}
