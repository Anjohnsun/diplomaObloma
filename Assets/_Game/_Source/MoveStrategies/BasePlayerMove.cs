using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMove : IMoveStrategy
{
    public List<Vector2Int> GetPossibleMoves(GridManager gridManager, Vector2Int currentPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // ходы
        Vector2Int[] possibleMoves = new Vector2Int[]
        {
            new Vector2Int(currentPosition.x + 1, currentPosition.y),
            new Vector2Int(currentPosition.x - 1, currentPosition.y),
            new Vector2Int(currentPosition.x, currentPosition.y + 1),
            new Vector2Int(currentPosition.x, currentPosition.y - 1),
        };

        // проверка на выход за границы поля
        foreach (var move in possibleMoves)
        {
            //if (gridManager.IsWithinBounds(move))
            {
                moves.Add(move);
            }
        }

        return moves;
    }
}
