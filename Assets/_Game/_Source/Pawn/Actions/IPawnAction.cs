using System.Collections.Generic;
using UnityEngine;

public interface IPawnAction
{
    List<Vector2Int> CalculateTargets();
    void Perform(Vector2Int point);
}
