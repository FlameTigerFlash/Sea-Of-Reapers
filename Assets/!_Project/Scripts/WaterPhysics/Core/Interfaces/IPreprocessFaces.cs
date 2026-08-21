using System.Collections.Generic;
using UnityEngine;
using WaterPhysics;

public interface IPreprocessFaces : IUpdatePosition
{
    public List<Polyhedron> GetPreprocessedFaces(in IReadOnlyList<Polyhedron> faces, WaterData water);
}
