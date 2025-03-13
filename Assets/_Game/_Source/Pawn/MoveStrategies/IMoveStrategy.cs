using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy
{
    List<Vector2Int> GetPossibleMoves(Vector3Int currentPosition);
}
