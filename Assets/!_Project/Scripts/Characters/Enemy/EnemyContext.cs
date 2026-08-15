using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyContext
{
    public UtilityBrain Brain;
    public Radar Radar;
    public Cannon Cannon;

    public ReactiveListener<GameObject> TargetObject;

    public readonly GameObject SelfObject;

    public readonly ReactiveField<Vector3> WaypointPosition = new();

    public readonly ReactiveField<float> ThrustMultiplier = new();
    public readonly ReactiveField<float> ThrustDirection = new();

    public ReactiveListener<float> Health;

    public readonly Dictionary<string, object> Data;

    public EnemyContext(GameObject self)
    {
        SelfObject = self;
    }
}
