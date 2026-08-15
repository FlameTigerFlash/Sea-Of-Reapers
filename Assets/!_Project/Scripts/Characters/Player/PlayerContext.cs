using UnityEngine;

public class PlayerContext
{
    public readonly GameObject SelfObject;

    public ReactiveField<Vector2> CameraMovement = new();
    public ReactiveField<Vector2> RequestedDirection = new();
    public ReactiveField<Vector2> AimingStickPosition = new();

    public ReactiveField<bool> IsAttacking = new();
    public ReactiveField<bool> IsAiming = new();
    public ReactiveField<bool> IsObserving = new();

    public ReactiveListener<float> Health;

    public PlayerContext(GameObject obj)
    {
        SelfObject = obj;
    }
}
