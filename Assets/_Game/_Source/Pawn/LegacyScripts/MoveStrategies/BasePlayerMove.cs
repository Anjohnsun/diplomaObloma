using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BasePlayerMove : IMoveStrategy
{
    public List<FieldTile> GetPossibleMoves(Vector2Int currentPosition)
    {
        List<FieldTile> moves = new List<FieldTile>();

        // ходы
        Vector2Int[] possibleMoves = new Vector2Int[]
        {
            new Vector2Int(currentPosition.x + 1, currentPosition.y),
            new Vector2Int(currentPosition.x - 1, currentPosition.y),
            new Vector2Int(currentPosition.x, currentPosition.y + 1),
            new Vector2Int(currentPosition.x, currentPosition.y - 1),
        };

        moves = GridManager.Instance.GetAvailableTargets(new List<Vector2Int>(possibleMoves), GridManager.IsFreeTile);

        return moves;
    }
}
