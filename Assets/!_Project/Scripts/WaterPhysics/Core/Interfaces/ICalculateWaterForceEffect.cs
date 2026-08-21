using System;
using System.Collections.Generic;

namespace WaterPhysics
{
    public interface ICalculateWaterForceEffect
    {
        public ForceEffectData CalculateForceEffect(List<ForceData> forces, WaterData water, RigidBodyData rb, float deltaTime = 1f);
    }
}