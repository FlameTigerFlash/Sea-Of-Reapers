using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Rotor : MonoBehaviour, IRotate
{
    [SerializeField, NotNull] private Transform _rotationPoint;

    [SerializeField, Range(0, 180)] private float _maxXRotation = 0;
    [SerializeField, Range(0, 180)] private float _maxYRotation = 90;
    [SerializeField, Range(0, 180)] private float _maxZRotation = 0;

    public Quaternion ParentRotation => _rotationPoint.parent.rotation;

    public Quaternion LocalRotation => _rotationPoint.localRotation;

    public Vector3 CurrentPosition => _rotationPoint.position;

    private Quaternion _desiredRotation;

    private void OnValidate()
    {
        _rotationPoint ??= transform;
    }

    private void Awake()
    {
        _desiredRotation = LocalRotation;
    }

    private void Update()
    {
        _rotationPoint.localRotation = Quaternion.Lerp(transform.localRotation, _desiredRotation, 1);
    }

    public void RotateTowards(Vector3 dir)
    {
        Quaternion globalRotation = Quaternion.LookRotation(dir, Vector3.up);
        _desiredRotation = ClampRotation(Quaternion.Inverse(ParentRotation) * globalRotation);
    }

    public void RotateLocally(Quaternion localRot)
    {
        localRot = ClampRotation(localRot);
        _desiredRotation = localRot;
    }

    private Quaternion ClampRotation(Quaternion localRotation)
    {
        Vector3 eulerAngles = localRotation.eulerAngles;

        if (_maxXRotation < 180)
        {
            eulerAngles.x = Mathf.Clamp(Mathf.DeltaAngle(0, eulerAngles.x), -_maxXRotation, _maxXRotation);
        }
        if (_maxYRotation < 180)
        {
            eulerAngles.y = Mathf.Clamp(Mathf.DeltaAngle(0, eulerAngles.y), -_maxYRotation, _maxYRotation);
        }
        if (_maxZRotation < 180)
        {
            eulerAngles.z = Mathf.Clamp(Mathf.DeltaAngle(0, eulerAngles.z), -_maxZRotation, _maxZRotation);
        }

        return Quaternion.Euler(eulerAngles);
    }
}
