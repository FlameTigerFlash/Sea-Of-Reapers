using System;
using System.Collections.Generic;
using UnityEngine;
using UtilitySystem;

namespace Character.Enemy
{
    public class ShipContext
    {
        public UtilityBrain Brain;
        public Radar Radar;
        public List<Cannon> Cannons;

        public ReactiveListener<GameObject> TargetObject;

        public readonly GameObject SelfObject;

        public readonly ReactiveField<Vector3> MovementDestination = new();

        public readonly ReactiveField<Vector3> WaypointPosition = new();

        public readonly ReactiveField<float> ThrustMultiplier = new();

        public readonly ReactiveField<float> ThrustDirection = new();

        public ReactiveListener<float> Health;

        public readonly Dictionary<string, object> Data;

        public ShipContext(GameObject self)
        {
            SelfObject = self;
        }
    }
}