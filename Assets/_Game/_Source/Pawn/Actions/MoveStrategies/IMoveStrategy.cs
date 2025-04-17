using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy
{
    List<FieldTile> GetPossibleMoves(Vector2Int currentPosition);
}
