using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy
{
    List<Vector2Int> GetPossibleMoves(GridManager gridManager, Vector2Int currentPosition);
}
