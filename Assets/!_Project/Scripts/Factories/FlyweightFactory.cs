using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

public class FlyweightFactory
{
    private MapLocator _mapLocator;

    private FlyweightPrefabsConfig _config = null;

    private Dictionary<GameObject, VisualObjectPool> _poolFromObject = new();

    private Dictionary<FlyweightTypes, VisualObjectPool> _pools = new();

    [Inject]
    public void Construct(FlyweightPrefabsConfig config, MapLocator mapLocator)
    {
        _mapLocator = mapLocator;
        _config = config;
    }

    public GameObject Create(FlyweightTypes type)
    {
        if (!_pools.ContainsKey(type))
        {
            var pool = new VisualObjectPool(_config.FlyweightPrefabs[type]);
            _pools[type] = pool;
        }

        var obj = _pools[type].Pool.Get();
        _poolFromObject[obj] = _pools[type];

        if (_mapLocator != null && obj.transform.parent != _mapLocator.FlyweightFolder)
        {
            obj.transform.SetParent(_mapLocator.FlyweightFolder);
        }

        return obj;
    }

    public bool TryRelease(GameObject obj)
    {
        if (!_poolFromObject.ContainsKey(obj))
        {
            return false;
        }
        _poolFromObject[obj].Pool.Release(obj);

        return true;
    }
}
