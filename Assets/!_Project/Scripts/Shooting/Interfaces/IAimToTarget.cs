using UnityEngine;

public interface IAimToTarget
{
    public Vector3 AimToTarget(Vector3 targetPos, out bool canHit);
}
