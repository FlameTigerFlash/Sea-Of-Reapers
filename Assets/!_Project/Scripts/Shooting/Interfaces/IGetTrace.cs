using System.Collections.Generic;
using UnityEngine;

public interface IGetTrace
{
    public List<TraceNode[]> GetTrace(Vector3 direction);
}
