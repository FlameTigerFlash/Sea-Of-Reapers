using System.Collections.Generic;
using WaterPhysics;

public interface ITransformForceEffect
{
    public ForceEffectData TransformForceEffect(ForceEffectData forceEffect, WaterData water, RigidBodyData rb, float deltaTime = 1f);
}
