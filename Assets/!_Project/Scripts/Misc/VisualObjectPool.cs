using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class VisualObjectPool
{
    private GameObject _prefab;

    private int _maxPoolSize = 1000;
    private bool _collectionChecks = true;

    private Dictionary<GameObject, Renderer> _renderers = new();

    private IObjectPool<GameObject> m_Pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, 10, _maxPoolSize);
            }
            return m_Pool;
        }
    }

    public VisualObjectPool(GameObject prefab)
    {
        _prefab = prefab;
    }

    public VisualObjectPool(GameObject prefab, int maxPoolSize, bool collectionChecks) : this(prefab)
    {
        _maxPoolSize = maxPoolSize;
        _collectionChecks = collectionChecks;
    }

    private GameObject CreatePooledItem()
    {
        var obj = MonoBehaviour.Instantiate(_prefab);
        _renderers[obj] = obj.GetComponentInChildren<Renderer>();
        return obj;
    }

    private void OnReturnedToPool(GameObject obj)
    {
        if (!_renderers.ContainsKey(obj))
        {
            _renderers[obj] = obj.GetComponentInChildren<Renderer>();
        }
        if (_renderers[obj] != null)
        {
            _renderers[obj].enabled = false;
        }
    }

    private void OnTakeFromPool(GameObject obj)
    {
        if (!_renderers.ContainsKey(obj))
        {
            _renderers[obj] = obj.GetComponentInChildren<Renderer>();
        }
        if (_renderers[obj] != null)
        {
            _renderers[obj].enabled = true;
        }
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        if (_renderers.ContainsKey(obj))
        {
            _renderers.Remove(obj);
        }
        MonoBehaviour.Destroy(obj);
    }
}
