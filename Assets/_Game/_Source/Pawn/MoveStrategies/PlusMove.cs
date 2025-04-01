using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlusMove : IMoveStrategy
{
    private GridManager _gridManager;

    public PlusMove(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    public List<Vector2Int> GetPossibleMoves(Vector3Int currentPosition)
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
            if (_gridManager.IsWithinBounds(move) && _gridManager.IsTileFree(move))
            {
                moves.Add(move);
            }
        }

        return moves;
    }
}