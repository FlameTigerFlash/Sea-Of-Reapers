using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private Transform _root = null;

    [SerializeField, Min(0.1f)] private float _fullScanTime = 2f;
    [SerializeField, Min(0)] private float _scanDistance = 5f;

    [SerializeField, Min(1)] private int _pointsCount = 5;

    [SerializeField] private bool _countSelfIntersections = false;

    [SerializeField] private bool _drawGizmos = false;

    public List<TraceRadarData> Traces => _traces;

    private List<TraceRadarData> _traces = new();
    private float _currentAngle = 0f;
    private int _currentIndex = 0;
    private int _cachedSize = 0;

    private void OnValidate()
    {
        if (_root == null)
        {
            var rb = GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                _root = rb.gameObject.transform;
            }
            else
            {
                _root = transform;
            }
        }
    }

    private void Awake()
    {
        _cachedSize = _pointsCount;
    }

    private void Update()
    {
        if (_cachedSize >= _pointsCount)
        {
            for (int i = _traces.Count - 1; i >= _pointsCount; i--)
            {
                _traces.RemoveAt(i);
            }
        }
        _cachedSize = _pointsCount;
        var scanStep = 360f / _pointsCount;

        _currentAngle = (_currentAngle + 360f / _fullScanTime * Time.deltaTime) % 360f;

        int index = Mathf.FloorToInt(_currentAngle / scanStep) % _pointsCount;

        if (index != _currentIndex)
        {
            _currentIndex = index;
            float angle = index * scanStep;
            if (index >= _traces.Count)
            {
                _traces.Add(ScanAngle(angle));
            }
            else
            {
                _traces[index] = ScanAngle(angle);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_traces == null || !_drawGizmos)
        {
            return;
        }

        Gizmos.color = Color.white;
        Vector3 waterPos = transform.position; waterPos.y = 0;
        Gizmos.DrawSphere(waterPos, 3f);

        foreach (var trace in _traces)
        {
            Gizmos.color = Color.green;
            var node = trace.Node;

            Vector3 pos = node.End;
            if (node.Hits != null && node.Hits.Length > 0)
            {
                Gizmos.color = Color.red;
                pos = node.Hits[0].point;
            }

            Gizmos.DrawSphere(pos, 2.5f);
            Gizmos.DrawLine(waterPos, pos);
        }
    }

    public TraceNode CheckDirection(Vector3 direction)
    {
        direction.y = 0;
        Vector3 waterPos = transform.position; waterPos.y = 0;

        Ray ray = new Ray(waterPos, direction);
        var hits = Physics.RaycastAll(ray, _scanDistance);
        if (!_countSelfIntersections)
        {
            int cnt = 0;
            List<RaycastHit> hitList = new();
            foreach (var hit in hits)
            {
                if (IsInSameGameObject(hit.transform))
                {
                    cnt++;
                    continue;
                }
                hitList.Add(hit);
            }
            if (cnt > 0)
            {
                hits = hitList.ToArray();
            }
        }
        TraceNode node = new TraceNode(); node.Start = waterPos; node.End = waterPos + direction * _scanDistance; node.Hits = hits;

        return node;
    }

    private TraceRadarData ScanAngle(float angle)
    {
        var node = CheckDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward);
        return new TraceRadarData(angle, ref node);
    }

    private bool IsInSameGameObject(Transform otherTransform)
    {
        if (otherTransform == transform)
        {
            return true;
        }
        if (_root == null)
        {
            return false;
        }
        while (otherTransform != null)
        {
            if (otherTransform == _root)
            {
                return true;
            }
            otherTransform = otherTransform.parent;
        }
        return false;
    }

    public class TraceRadarData
    {
        public readonly float Angle;
        public readonly TraceNode Node;

        public TraceRadarData(float angle, ref TraceNode node)
        {
            Angle = angle;
            Node = node;
        }
    }

    protected class TraceComparer : IComparer<TraceRadarData>
    {
        public int Compare(TraceRadarData a, TraceRadarData b)
        {
            if (a == null || b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            return a.Angle.CompareTo(b.Angle);
        }
    }
}