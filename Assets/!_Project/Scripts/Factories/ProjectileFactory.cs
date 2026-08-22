using System;
using System.Collections.Generic;
using System.Text;
using Zenject;
using UnityEngine;

public class ProjectileFactory
{
    private DiContainer _container;
    private MapLocator _mapLocator;

    private ProjectilePrefabsConfig _config = null;

    [Inject]
    public void Construct(DiContainer container, ProjectilePrefabsConfig config, MapLocator mapLocator)
    {
        _mapLocator = mapLocator;
        _config = config;
        _container = container;
    }

    public GameObject Create(ProjectileTypes type, Vector3 position, Quaternion rotation)
    {
        if (!_config.ProjectilePrefabs.ContainsKey(type))
        {
            throw new ArgumentException($"Type {type} does not provide a prefab to spawn.");
        }
        GameObject projectilePrefab = _config.ProjectilePrefabs[type];

        return _container.InstantiatePrefab(projectilePrefab, position, rotation, _mapLocator != null ? _mapLocator.ProjectilesFolder : null);
    }
}
