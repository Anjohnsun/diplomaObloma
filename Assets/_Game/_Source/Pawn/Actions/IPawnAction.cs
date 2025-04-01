using System.Collections.Generic;
using UnityEngine;

public interface IPawnAction
{
    Pawn pawn { get; }
    float duration { get; }
    List<Vector2Int> CalculateTargets();
    void Perform(Vector2Int point);
}
