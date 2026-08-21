using System.Collections.Generic;
using WaterPhysics;

public interface ITransformForces
{
    public List<ForceData> TransformForces(List<ForceData> forces, WaterData water, RigidBodyData rb, float deltaTime = 1f);
}
