using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using System;

public class Cannon : MonoBehaviour, IShoot, IRotate, IGetTrace, IAimToTarget
{
    #region fields
    [SerializeField, NotNull] private Transform _shootingPoint;

    [Header("Rotation")]
    [SerializeField, Range(-90, 90)] private float _minVerticalRotation = -80;
    [SerializeField, Range(-90, 90)] private float _maxVerticalRotation = -10;
    [SerializeField, Range(0, 90)] private float _maxHorizontalRotation = 90;
    [SerializeField, Min(0)] private float _rotationSpeed = 4f;

    [Header("Shooting settings")]
    [SerializeField] private GameObject _projectile;
    [SerializeField, Min(float.Epsilon)] private float _shootingDelay = 2f;
    [SerializeField] private float _shootingAcceleration = 20f;

    [Header("Trace settings")]
    [SerializeField, Range(2, 50)] private int _pointsCount = 2;

    public Quaternion ParentRotation => transform.parent.rotation;
    public Quaternion LocalRotation => transform.localRotation;
    public Quaternion GlobalRotation => ParentRotation * LocalRotation;

    public Vector3 CurrentPosition => transform.position;

    public bool CanShoot { get; private set; } = true;

    private Quaternion _desiredRotation = Quaternion.identity;
    #endregion

    private void OnValidate()
    {
        if (_maxVerticalRotation < _minVerticalRotation)
        {
            (_maxVerticalRotation, _minVerticalRotation) = (_minVerticalRotation, _maxVerticalRotation);
        }
    }

    private void Awake()
    {
        RotateTowards(Vector3.forward);
    }

    private void Update()
    {
        float t = 1f - Mathf.Exp(-_rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _desiredRotation, t);
    }

    public bool TryShoot()
    {
        if (!CanShoot)
        {
            return false;
        }

        Shoot();
        StartCoroutine(DelayCoroutine(_shootingDelay));

        return true;
    }

    public void RotateTowards(Vector3 direction)
    {
        Vector3 targetRotation = NormalizedEulerAngles(Quaternion.LookRotation(direction, transform.up).eulerAngles - transform.parent.rotation.eulerAngles);
        targetRotation.x = Mathf.Clamp(targetRotation.x, _minVerticalRotation, _maxVerticalRotation);
        targetRotation.y = Mathf.Clamp(targetRotation.y, -_maxHorizontalRotation, _maxHorizontalRotation);
        targetRotation.z = 0;

        _desiredRotation = Quaternion.Euler(targetRotation);
    }

    public void RotateLocally(Quaternion newLocalRot)
    {
        Vector3 eulerRot = NormalizedEulerAngles(newLocalRot.eulerAngles);
        Vector3 targetRotation;
        targetRotation.x = Mathf.Clamp(eulerRot.x, _minVerticalRotation, _maxVerticalRotation);
        targetRotation.y = Mathf.Clamp(eulerRot.y, -_maxHorizontalRotation, _maxHorizontalRotation);
        targetRotation.z = 0;

        _desiredRotation = Quaternion.Euler(targetRotation);
    }

    public Vector3 AimToTarget(Vector3 targetPos)
    {
        float angle = 45;
        Vector3 connector = targetPos - CurrentPosition, horizontalDirection = new Vector3(connector.x, 0, connector.z);
        float relHeight = connector.y;

        float b = horizontalDirection.magnitude,
            a = -(Physics.gravity.magnitude * horizontalDirection.sqrMagnitude / (2 * _shootingAcceleration * _shootingAcceleration)),
            c = a - relHeight;

        float D = b * b - 4 * a * c;
        if (D < 0)
        {
            angle = 45 * Mathf.Deg2Rad;
        }
        else
        {
            float tan = (-b + Mathf.Sqrt(D)) / (a * 2);
            angle = Mathf.Atan(tan);
        }
        var direction = horizontalDirection.normalized * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);

        return direction;
    }

    public List<TraceNode[]> GetTrace(Vector3 direction)
    {
        var ret = new List<TraceNode[]>();
        if (_pointsCount < 2)
        {
            return ret;
        }
        TraceNode[] trace = new TraceNode[_pointsCount-1];

        Vector3 startPoint = _shootingPoint.position;
        Vector3 movement = direction * _shootingAcceleration;
        Vector3 verticalComponent = Vector3.Project(movement, Vector3.up), horizontalComponent = movement - verticalComponent;
        float verticalSpeed = verticalComponent.y, initialHeight = _shootingPoint.position.y, g = Physics.gravity.magnitude;

        float tEnd = (verticalSpeed + Mathf.Sqrt(verticalSpeed * verticalSpeed + 2 * g * initialHeight)) / g;
        float timestep = (tEnd / (_pointsCount - 1));
        for (int cnt = 1; cnt < _pointsCount; cnt++)
        {
            float t = timestep * cnt;
            Vector3 endPoint = horizontalComponent * t + _shootingPoint.position;
            endPoint.y += verticalSpeed * t - g * t * t / 2;

            Ray ray = new Ray(startPoint, endPoint - startPoint);
            RaycastHit[] hits = Physics.RaycastAll(ray, (endPoint - startPoint).magnitude);

            TraceNode node = new TraceNode(); node.Start = startPoint; node.End = endPoint; node.Hits = hits;
            trace[cnt-1] = node; 
            startPoint = endPoint;
        }
        ret.Add(trace);
        return ret;
    }

    private void Shoot()
    {
        var proj = Instantiate(_projectile, _shootingPoint.position, _shootingPoint.rotation);
        var rb = proj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile must have a Rigid Body component!");
            Destroy(proj);
        }

        rb.AddForce(_shootingPoint.forward * _shootingAcceleration, ForceMode.VelocityChange);
    }

    private Vector3 NormalizedEulerAngles(Vector3 eulerAngles)
    {
        eulerAngles.x = Mathf.DeltaAngle(0, eulerAngles.x);
        eulerAngles.y = Mathf.DeltaAngle(0, eulerAngles.y);
        eulerAngles.z = Mathf.DeltaAngle(0, eulerAngles.z);
        return eulerAngles;
    }

    private IEnumerator DelayCoroutine(float delay)
    {
        CanShoot = false;
        yield return new WaitForSeconds(delay);
        CanShoot = true;
    }
}
